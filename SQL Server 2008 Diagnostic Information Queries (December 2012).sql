
-- SQL Server 2008 and R2 Diagnostic Information Queries
-- Glenn Berry 
-- December 2012
-- Last Modified: December 29, 2012
-- http://sqlserverperformance.wordpress.com/
-- http://sqlskills.com/blogs/glenn/
-- Twitter: GlennAlanBerry

-- Note: A number of these queries will only work on SQL Server 2008 R2 SP1 or later
-- They are all noted in the instructions

-- Instance level queries *******************************

-- SQL and OS Version information for current instance  (Query 1)(Version Info)
SELECT @@SERVERNAME AS [Server Name], @@VERSION AS [SQL Server and OS Version Info];

-- SQL Server 2008 RTM is considered an "unsupported service pack" as of April 13, 2010
-- SQL Server 2008 SP1 is considered an "unsupported service pack" as of September 19, 2011
-- SQL Server 2008 SP2 is considered an "unsupported service pack" as of September 17, 2012

-- When was SQL Server installed  (Query 2)(SQL Server Install Date)   
SELECT @@SERVERNAME AS [Server Name], createdate AS [SQL Server Install Date] 
FROM sys.syslogins WITH (NOLOCK)
WHERE [sid] = 0x010100000000000512000000;

-- Tells you the date and time that SQL Server was installed
-- It is a good idea to know how old your instance is


-- Get selected server properties (SQL Server 2008)  (Query 3)(Server Properties)
SELECT SERVERPROPERTY('MachineName') AS [MachineName], SERVERPROPERTY('ServerName') AS [ServerName],  
SERVERPROPERTY('InstanceName') AS [Instance], SERVERPROPERTY('IsClustered') AS [IsClustered], 
SERVERPROPERTY('ComputerNamePhysicalNetBIOS') AS [ComputerNamePhysicalNetBIOS], 
SERVERPROPERTY('Edition') AS [Edition], SERVERPROPERTY('ProductLevel') AS [ProductLevel], 
SERVERPROPERTY('ProductVersion') AS [ProductVersion], SERVERPROPERTY('ProcessID') AS [ProcessID],
SERVERPROPERTY('Collation') AS [Collation], SERVERPROPERTY('IsFullTextInstalled') AS [IsFullTextInstalled], 
SERVERPROPERTY('IsIntegratedSecurityOnly') AS [IsIntegratedSecurityOnly];

-- This gives you a lot of useful information about your instance of SQL Server


-- Windows information (SQL Server 2008 R2 SP1 or greater)  (Query 4)(Windows Info)
SELECT windows_release, windows_service_pack_level, 
       windows_sku, os_language_version
FROM sys.dm_os_windows_info WITH (NOLOCK) OPTION (RECOMPILE);

-- Gives you major OS version, Service Pack, Edition, and language info for the operating system


-- SQL Server Services information (SQL Server 2008 R2 SP1 or greater)  (Query 5)(SQL Server Services Info)
SELECT servicename, startup_type_desc, status_desc, 
last_startup_time, service_account, is_clustered, cluster_nodename
FROM sys.dm_server_services WITH (NOLOCK) OPTION (RECOMPILE);

-- Tells you the account being used for the SQL Server Service and the SQL Agent Service
-- Shows when they were last started, and their current status
-- Shows whether you are running on a failover cluster



-- SQL Server NUMA Node information  (Query 6)(SQL Server NUMA Info)
SELECT node_id, node_state_desc, memory_node_id, online_scheduler_count, 
       active_worker_count, avg_load_balance 
FROM sys.dm_os_nodes WITH (NOLOCK) 
WHERE node_state_desc <> N'ONLINE DAC' OPTION (RECOMPILE);

-- Gives you some useful information about the composition 
-- and relative load on your NUMA nodes




-- Hardware information from SQL Server 2008 and 2008 R2  (Query 7)(Hardware Info)
-- (Cannot distinguish between HT and multi-core)
SELECT cpu_count AS [Logical CPU Count], hyperthread_ratio AS [Hyperthread Ratio],
cpu_count/hyperthread_ratio AS [Physical CPU Count], 
physical_memory_in_bytes/1048576 AS [Physical Memory (MB)], 
sqlserver_start_time --, affinity_type_desc -- (affinity_type_desc is only in 2008 R2)
FROM sys.dm_os_sys_info WITH (NOLOCK) OPTION (RECOMPILE);

-- Gives you some good basic hardware information about your database server


-- Get System Manufacturer and model number from  (Query 8)(System Manufacturer) 
-- SQL Server Error log. This query might take a few seconds 
-- if you have not recycled your error log recently
EXEC xp_readerrorlog 0,1,"Manufacturer"; 

-- This can help you determine the capabilities
-- and capacities of your database server


-- Get processor description from Windows Registry  (Query 9)(Processor Description)
EXEC xp_instance_regread 
'HKEY_LOCAL_MACHINE', 'HARDWARE\DESCRIPTION\System\CentralProcessor\0',
'ProcessorNameString';

-- Gives you the model number and rated clock speed of your processor(s)
-- Your processors may be running at less that the rated clock speed due
-- to the Windows Power Plan or hardware power management


-- Get the current node name from your cluster nodes  (Query 10)(Current Cluster Node)
-- (if your database server is in a cluster)
SELECT NodeName
FROM sys.dm_os_cluster_nodes WITH (NOLOCK) OPTION (RECOMPILE);

-- Knowing which node owns the cluster resources is critical
-- Especially when you are installing Windows or SQL Server updates
-- You will see no results if your instance is not clustered


-- Get configuration values for instance  (Query 11)(Configuration Values)
SELECT name, value, value_in_use, [description] 
FROM sys.configurations WITH (NOLOCK)
ORDER BY name OPTION (RECOMPILE);

-- Focus on
-- backup compression default
-- clr enabled (only enable if it is needed)
-- lightweight pooling (should be zero)
-- max degree of parallelism (depends on your workload)
-- max server memory (MB) (set to an appropriate value)
-- optimize for ad hoc workloads (should be 1)
-- priority boost (should be zero)


-- SQL Server Registry information (SQL Server 2008 R2 SP1 or greater)  (Query 12)(SQL Server Registry Info)
SELECT registry_key, value_name, value_data
FROM sys.dm_server_registry WITH (NOLOCK) OPTION (RECOMPILE);

-- This lets you safely read some SQL Server related 
-- information from the Windows Registry


-- Get information on location, time and size of any memory dumps from SQL Server (SQL Server 2008 R2 SP1 or greater)  (Query 13)(Memory Dump Info)
SELECT [filename], creation_time, size_in_bytes
FROM sys.dm_server_memory_dumps WITH (NOLOCK) OPTION (RECOMPILE);

-- This will not return any rows if you have 
-- not had any memory dumps (which is a good thing)


-- File Names and Paths for TempDB and all user databases in instance  (Query 14)(Database Filenames and Paths)
SELECT DB_NAME([database_id])AS [Database Name], 
       [file_id], name, physical_name, type_desc, state_desc, 
       CONVERT( bigint, size/128.0) AS [Total Size in MB]
FROM sys.master_files WITH (NOLOCK)
WHERE [database_id] > 4 
AND [database_id] <> 32767
OR [database_id] = 2
ORDER BY DB_NAME([database_id]) OPTION (RECOMPILE);

-- Things to look at:
-- Are data files and log files on different drives?
-- Is everything on the C: drive?
-- Is TempDB on dedicated drives?
-- Is there only one TempDB data file?
-- Are all of the TempDB data files the same size?
-- Are there multiple data files for user databases?


-- Volume info for all databases on the current instance (SQL Server 2008 R2 SP1 or greater)  (Query 15)(Volume Info)
SELECT DB_NAME(f.database_id) AS [DatabaseName], f.file_id, 
vs.volume_mount_point, vs.total_bytes, vs.available_bytes, 
CAST(CAST(vs.available_bytes AS FLOAT)/ CAST(vs.total_bytes AS FLOAT) AS DECIMAL(18,3)) * 100 AS [Space Free %]
FROM sys.master_files AS f WITH (NOLOCK)
CROSS APPLY sys.dm_os_volume_stats(f.database_id, f.file_id) AS vs
ORDER BY f.database_id OPTION (RECOMPILE);

--Shows you the free space on the LUNs where you have database data or log files


-- Recovery model, log reuse wait description, log file size, log usage size  (Query 16)(Database Properties) 
-- and compatibility level for all databases on instance
SELECT db.[name] AS [Database Name], db.recovery_model_desc AS [Recovery Model], 
db.log_reuse_wait_desc AS [Log Reuse Wait Description], 
ls.cntr_value AS [Log Size (KB)], lu.cntr_value AS [Log Used (KB)],
CAST(CAST(lu.cntr_value AS FLOAT) / CAST(ls.cntr_value AS FLOAT)AS DECIMAL(18,2)) * 100 AS [Log Used %], 
db.[compatibility_level] AS [DB Compatibility Level], 
db.page_verify_option_desc AS [Page Verify Option], db.is_auto_create_stats_on, db.is_auto_update_stats_on,
db.is_auto_update_stats_async_on, db.is_parameterization_forced, 
db.snapshot_isolation_state_desc, db.is_read_committed_snapshot_on,
db.is_auto_close_on, db.is_auto_shrink_on
FROM sys.databases AS db WITH (NOLOCK)
INNER JOIN sys.dm_os_performance_counters AS lu WITH (NOLOCK)
ON db.name = lu.instance_name
INNER JOIN sys.dm_os_performance_counters AS ls WITH (NOLOCK) 
ON db.name = ls.instance_name
WHERE lu.counter_name LIKE N'Log File(s) Used Size (KB)%' 
AND ls.counter_name LIKE N'Log File(s) Size (KB)%'
AND ls.cntr_value > 0 OPTION (RECOMPILE);


-- Things to look at:
-- How many databases are on the instance?
-- What recovery models are they using?
-- What is the log reuse wait description?
-- How full are the transaction logs ?
-- What compatibility level are they on?
-- What is the Page Verify Option?
-- Make sure auto_shrink and auto_close are not enabled!




-- Get VLF Counts for all databases on the instance (Query 17)(VLF Counts)
-- (adapted from Michelle Ufford) 
CREATE TABLE #VLFInfo (FileID  int,
					   FileSize bigint, StartOffset bigint,
					   FSeqNo      bigint, [Status]    bigint,
					   Parity      bigint, CreateLSN   numeric(38));
	 
CREATE TABLE #VLFCountResults(DatabaseName sysname, VLFCount int);
	 
EXEC sp_MSforeachdb N'Use [?]; 

				INSERT INTO #VLFInfo 
				EXEC sp_executesql N''DBCC LOGINFO([?])''; 
	 
				INSERT INTO #VLFCountResults 
				SELECT DB_NAME(), COUNT(*) 
				FROM #VLFInfo; 

				TRUNCATE TABLE #VLFInfo;'
	 
SELECT DatabaseName, VLFCount  
FROM #VLFCountResults
ORDER BY VLFCount DESC;
	 
DROP TABLE #VLFInfo;
DROP TABLE #VLFCountResults;

-- High VLF counts can affect write performance 
-- and they can make database restores and recovery take much longer



-- Calculates average stalls per read, per write, and per total input/output for each database file  (Query 18)(IO Stalls by File) 
SELECT DB_NAME(fs.database_id) AS [Database Name], mf.physical_name, io_stall_read_ms, num_of_reads,
CAST(io_stall_read_ms/(1.0 + num_of_reads) AS NUMERIC(10,1)) AS [avg_read_stall_ms],io_stall_write_ms, 
num_of_writes,CAST(io_stall_write_ms/(1.0+num_of_writes) AS NUMERIC(10,1)) AS [avg_write_stall_ms],
io_stall_read_ms + io_stall_write_ms AS [io_stalls], num_of_reads + num_of_writes AS [total_io],
CAST((io_stall_read_ms + io_stall_write_ms)/(1.0 + num_of_reads + num_of_writes) AS NUMERIC(10,1)) 
AS [avg_io_stall_ms]
FROM sys.dm_io_virtual_file_stats(null,null) AS fs
INNER JOIN sys.master_files AS mf WITH (NOLOCK)
ON fs.database_id = mf.database_id
AND fs.[file_id] = mf.[file_id]
ORDER BY avg_io_stall_ms DESC OPTION (RECOMPILE);

-- Helps you determine which database files on the entire instance have the most I/O bottlenecks
-- This can help you decide whether certain LUNs are overloaded and whether you might
-- want to move some files to a different location



-- Get CPU utilization by database (adapted from Robert Pearl)  (Query 19)(CPU Usage By Database)
WITH DB_CPU_Stats
AS
(SELECT DatabaseID, DB_Name(DatabaseID) AS [DatabaseName], SUM(total_worker_time) AS [CPU_Time_Ms]
 FROM sys.dm_exec_query_stats AS qs
 CROSS APPLY (SELECT CONVERT(int, value) AS [DatabaseID] 
              FROM sys.dm_exec_plan_attributes(qs.plan_handle)
              WHERE attribute = N'dbid') AS F_DB
 GROUP BY DatabaseID)
SELECT ROW_NUMBER() OVER(ORDER BY [CPU_Time_Ms] DESC) AS [row_num],
       DatabaseName, [CPU_Time_Ms], 
       CAST([CPU_Time_Ms] * 1.0 / SUM([CPU_Time_Ms]) OVER() * 100.0 AS DECIMAL(5, 2)) AS [CPUPercent]
FROM DB_CPU_Stats
WHERE DatabaseID > 4 -- system databases
AND DatabaseID <> 32767 -- ResourceDB
ORDER BY row_num OPTION (RECOMPILE);

-- Helps determine which database is using the most CPU resources on the instance


-- Get total buffer usage by database for current instance  (Query 20)(Total Buffer Usage by Database)
-- This make take some time to run on a busy instance
SELECT DB_NAME(database_id) AS [Database Name],
COUNT(*) * 8/1024.0 AS [Cached Size (MB)]
FROM sys.dm_os_buffer_descriptors WITH (NOLOCK)
WHERE database_id > 4 -- system databases
AND database_id <> 32767 -- ResourceDB
GROUP BY DB_NAME(database_id)
ORDER BY [Cached Size (MB)] DESC OPTION (RECOMPILE);

-- Tells you how much memory (in the buffer pool) 
-- is being used by each database on the instance


-- Clear Wait Stats 
-- DBCC SQLPERF('sys.dm_os_wait_stats', CLEAR);

-- Isolate top waits for server instance since last restart or statistics clear  (Query 21)(Top Waits)
WITH Waits AS
(SELECT wait_type, wait_time_ms / 1000. AS wait_time_s,
100. * wait_time_ms / SUM(wait_time_ms) OVER() AS pct,
ROW_NUMBER() OVER(ORDER BY wait_time_ms DESC) AS rn
FROM sys.dm_os_wait_stats WITH (NOLOCK)
WHERE wait_type NOT IN (N'CLR_SEMAPHORE',N'LAZYWRITER_SLEEP',N'RESOURCE_QUEUE',N'SLEEP_TASK',
N'SLEEP_SYSTEMTASK',N'SQLTRACE_BUFFER_FLUSH',N'WAITFOR', N'LOGMGR_QUEUE',N'CHECKPOINT_QUEUE',
N'REQUEST_FOR_DEADLOCK_SEARCH',N'XE_TIMER_EVENT',N'BROKER_TO_FLUSH',N'BROKER_TASK_STOP',N'CLR_MANUAL_EVENT',
N'CLR_AUTO_EVENT',N'DISPATCHER_QUEUE_SEMAPHORE', N'FT_IFTS_SCHEDULER_IDLE_WAIT',
N'XE_DISPATCHER_WAIT', N'XE_DISPATCHER_JOIN', N'SQLTRACE_INCREMENTAL_FLUSH_SLEEP',
N'ONDEMAND_TASK_QUEUE', N'BROKER_EVENTHANDLER', N'SLEEP_BPOOL_FLUSH'))
SELECT W1.wait_type, 
CAST(W1.wait_time_s AS DECIMAL(12, 2)) AS wait_time_s,
CAST(W1.pct AS DECIMAL(12, 2)) AS pct,
CAST(SUM(W2.pct) AS DECIMAL(12, 2)) AS running_pct
FROM Waits AS W1
INNER JOIN Waits AS W2
ON W2.rn <= W1.rn
GROUP BY W1.rn, W1.wait_type, W1.wait_time_s, W1.pct
HAVING SUM(W2.pct) - W1.pct < 99 OPTION (RECOMPILE); -- percentage threshold

-- Common Significant Wait types with BOL explanations

-- *** Network Related Waits ***
-- ASYNC_NETWORK_IO		Occurs on network writes when the task is blocked behind the network

-- *** Locking Waits ***
-- LCK_M_IX				Occurs when a task is waiting to acquire an Intent Exclusive (IX) lock
-- LCK_M_IU				Occurs when a task is waiting to acquire an Intent Update (IU) lock
-- LCK_M_S				Occurs when a task is waiting to acquire a Shared lock

-- *** I/O Related Waits ***
-- ASYNC_IO_COMPLETION  Occurs when a task is waiting for I/Os to finish
-- IO_COMPLETION		Occurs while waiting for I/O operations to complete. 
--                      This wait type generally represents non-data page I/Os. Data page I/O completion waits appear 
--                      as PAGEIOLATCH_* waits
-- PAGEIOLATCH_SH		Occurs when a task is waiting on a latch for a buffer that is in an I/O request. 
--                      The latch request is in Shared mode. Long waits may indicate problems with the disk subsystem.
-- PAGEIOLATCH_EX		Occurs when a task is waiting on a latch for a buffer that is in an I/O request. 
--                      The latch request is in Exclusive mode. Long waits may indicate problems with the disk subsystem.
-- WRITELOG             Occurs while waiting for a log flush to complete. 
--                      Common operations that cause log flushes are checkpoints and transaction commits.
-- PAGELATCH_EX			Occurs when a task is waiting on a latch for a buffer that is not in an I/O request. 
--                      The latch request is in Exclusive mode.
-- BACKUPIO				Occurs when a backup task is waiting for data, or is waiting for a buffer in which to store data

-- *** CPU Related Waits ***
-- SOS_SCHEDULER_YIELD  Occurs when a task voluntarily yields the scheduler for other tasks to execute. 
--                      During this wait the task is waiting for its quantum to be renewed.

-- THREADPOOL			Occurs when a task is waiting for a worker to run on. 
--                      This can indicate that the maximum worker setting is too low, or that batch executions are taking 
--                      unusually long, thus reducing the number of workers available to satisfy other batches.
-- CX_PACKET			Occurs when trying to synchronize the query processor exchange iterator 
--						You may consider lowering the degree of parallelism if contention on this wait type becomes a problem
--						Often caused by missing indexes or poorly written queries


-- Signal Waits for instance  (Query 22)(Signal Waits)
SELECT CAST(100.0 * SUM(signal_wait_time_ms) / SUM (wait_time_ms) AS NUMERIC(20,2)) 
AS [%signal (cpu) waits],
CAST(100.0 * SUM(wait_time_ms - signal_wait_time_ms) / SUM (wait_time_ms) AS NUMERIC(20,2)) 
AS [%resource waits]
FROM sys.dm_os_wait_stats WITH (NOLOCK) OPTION (RECOMPILE);

-- Signal Waits above 10-15% is usually a sign of CPU pressure
-- Resource waits are non-CPU related waits


--  Get logins that are connected and how many sessions they have  (Query 23)(Connection Counts) 
SELECT login_name, COUNT(session_id) AS [session_count] 
FROM sys.dm_exec_sessions WITH (NOLOCK)
WHERE session_id > 50	-- filter out system SPIDs
GROUP BY login_name
ORDER BY COUNT(session_id) DESC OPTION (RECOMPILE);

-- This can help characterize your workload and
-- determine whether you are seeing a normal level of activity


-- Get Average Task Counts (run multiple times)  (Query 24)(Avg Task Counts)
SELECT AVG(current_tasks_count) AS [Avg Task Count], 
AVG(runnable_tasks_count) AS [Avg Runnable Task Count],
AVG(pending_disk_io_count) AS [AvgPendingDiskIOCount]
FROM sys.dm_os_schedulers WITH (NOLOCK)
WHERE scheduler_id < 255 OPTION (RECOMPILE);

-- Sustained values above 10 suggest further investigation in that area
-- High current_tasks_count is often an indication of locking/blocking problems
-- High runnable_tasks_count is an indication of CPU pressure
-- High pending_disk_io_count is an indication of I/O pressure


-- Get CPU Utilization History for last 256 minutes (in one minute intervals)  (Query 25)(CPU Utilization History)
-- This version works with SQL Server 2008 and SQL Server 2008 R2 only
DECLARE @ts_now bigint = (SELECT cpu_ticks/(cpu_ticks/ms_ticks)FROM sys.dm_os_sys_info); 

SELECT TOP(256) SQLProcessUtilization AS [SQL Server Process CPU Utilization], 
               SystemIdle AS [System Idle Process], 
               100 - SystemIdle - SQLProcessUtilization AS [Other Process CPU Utilization], 
               DATEADD(ms, -1 * (@ts_now - [timestamp]), GETDATE()) AS [Event Time] 
FROM ( 
	  SELECT record.value('(./Record/@id)[1]', 'int') AS record_id, 
			record.value('(./Record/SchedulerMonitorEvent/SystemHealth/SystemIdle)[1]', 'int') 
			AS [SystemIdle], 
			record.value('(./Record/SchedulerMonitorEvent/SystemHealth/ProcessUtilization)[1]', 
			'int') 
			AS [SQLProcessUtilization], [timestamp] 
	  FROM ( 
			SELECT [timestamp], CONVERT(xml, record) AS [record] 
			FROM sys.dm_os_ring_buffers WITH (NOLOCK)
			WHERE ring_buffer_type = N'RING_BUFFER_SCHEDULER_MONITOR' 
			AND record LIKE N'%<SystemHealth>%') AS x 
	  ) AS y 
ORDER BY record_id DESC OPTION (RECOMPILE);

-- Look at the trend over the entire period. 
-- Also look at high sustained Other Process CPU Utilization values


-- Good basic information about OS memory amounts and state  (Query 26)(System Memory)
SELECT total_physical_memory_kb, available_physical_memory_kb, 
       total_page_file_kb, available_page_file_kb, 
       system_memory_state_desc
FROM sys.dm_os_sys_memory WITH (NOLOCK) OPTION (RECOMPILE);

-- You want to see "Available physical memory is high"
-- This indicates that you are not under external memory pressure


-- SQL Server Process Address space info  (Query 27)(Process Memory) 
--(shows whether locked pages is enabled, among other things)
SELECT physical_memory_in_use_kb,locked_page_allocations_kb, 
       page_fault_count, memory_utilization_percentage, 
       available_commit_limit_kb, process_physical_memory_low, 
       process_virtual_memory_low
FROM sys.dm_os_process_memory WITH (NOLOCK) OPTION (RECOMPILE);

-- You want to see 0 for process_physical_memory_low
-- You want to see 0 for process_virtual_memory_low
-- This indicates that you are not under internal memory pressure


-- Page Life Expectancy (PLE) value for each NUMA node in current instance  (Query 28)(PLE by Node)
SELECT @@SERVERNAME AS [Server Name], [object_name], instance_name, cntr_value AS [Page Life Expectancy]
FROM sys.dm_os_performance_counters WITH (NOLOCK)
WHERE [object_name] LIKE N'%Buffer Node%' -- Handles named instances
AND counter_name = N'Page life expectancy' OPTION (RECOMPILE);

-- PLE is a good measurement of memory pressure.
-- Higher PLE is better. Watch the trend, not the absolute value.
-- This will only return one row for non-NUMA systems.


-- Memory Grants Pending value for current instance  (Query 29)(Memory Grants Pending)
SELECT @@SERVERNAME AS [Server Name], [object_name], cntr_value AS [Memory Grants Pending]                                                                                                       
FROM sys.dm_os_performance_counters WITH (NOLOCK)
WHERE [object_name] LIKE N'%Memory Manager%' -- Handles named instances
AND counter_name = N'Memory Grants Pending' OPTION (RECOMPILE);

-- Memory Grants Pending above zero for a sustained period is a very strong indicator of memory pressure


-- Memory Clerk Usage for instance  (Query 30)(Memory Clerk Usage)
-- Look for high value for CACHESTORE_SQLCP (Ad-hoc query plans)
SELECT TOP(10) [type] AS [Memory Clerk Type], SUM(single_pages_kb) AS [SPA Mem, Kb] 
FROM sys.dm_os_memory_clerks WITH (NOLOCK)
GROUP BY [type]  
ORDER BY SUM(single_pages_kb) DESC OPTION (RECOMPILE);

-- CACHESTORE_SQLCP  SQL Plans         
-- These are cached SQL statements or batches that 
-- aren't in stored procedures, functions and triggers
--
-- CACHESTORE_OBJCP  Object Plans      
-- These are compiled plans for 
-- stored procedures, functions and triggers
--
-- CACHESTORE_PHDR   Algebrizer Trees  
-- An algebrizer tree is the parsed SQL text 
-- that resolves the table and column names


-- Find single-use, ad-hoc and prepared queries that are bloating the plan cache  (Query 31)(Ad-hoc Queries)
SELECT TOP(50) [text] AS [QueryText], cp.objtype, cp.size_in_bytes
FROM sys.dm_exec_cached_plans AS cp WITH (NOLOCK)
CROSS APPLY sys.dm_exec_sql_text(plan_handle) 
WHERE cp.cacheobjtype = N'Compiled Plan' 
AND cp.objtype IN (N'Adhoc', N'Prepared') 
AND cp.usecounts = 1
ORDER BY cp.size_in_bytes DESC OPTION (RECOMPILE);

-- Gives you the text and size of single-use ad-hoc queries  that waste space in the plan cache
-- Enabling 'optimize for ad hoc workloads' for the instance can help (SQL Server 2008 and 2008 R2 only)
-- Running DBCC FREESYSTEMCACHE ('SQL Plans') periodically may be required to better control this.
-- Enabling forced parameterization for the database can help, but test first!


-- Database specific queries *****************************************************************

-- **** Switch to a user database *****
USE YourDatabaseName;
GO

-- Individual File Sizes and space available for current database  (Query 32)(File Sizes+Space)
SELECT f.name AS [File Name] , f.physical_name AS [Physical Name], 
CAST((f.size/128.0) AS decimal(15,2)) AS [Total Size in MB],
CAST(f.size/128.0 - CAST(FILEPROPERTY(f.name, 'SpaceUsed') AS int)/128.0 AS decimal(15,2)) 
AS [Available Space In MB], [file_id], fg.name AS [Filegroup Name]
FROM sys.database_files AS f WITH (NOLOCK) 
LEFT OUTER JOIN sys.data_spaces AS fg WITH (NOLOCK) 
ON f.data_space_id = fg.data_space_id OPTION (RECOMPILE);

-- Look at how large and how full the files are and where they are located
-- Make sure the transaction log is not full!!



-- I/O Statistics by file for the current database  (Query 33)(IO Stats By File)
SELECT DB_NAME(DB_ID()) AS [Database Name],[file_id], num_of_reads, num_of_writes, 
io_stall_read_ms, io_stall_write_ms,
CAST(100. * io_stall_read_ms/(io_stall_read_ms + io_stall_write_ms) AS DECIMAL(10,1)) AS [IO Stall Reads Pct],
CAST(100. * io_stall_write_ms/(io_stall_write_ms + io_stall_read_ms) AS DECIMAL(10,1)) AS [IO Stall Writes Pct],
(num_of_reads + num_of_writes) AS [Writes + Reads], num_of_bytes_read, num_of_bytes_written,
CAST(100. * num_of_reads/(num_of_reads + num_of_writes) AS DECIMAL(10,1)) AS [# Reads Pct],
CAST(100. * num_of_writes/(num_of_reads + num_of_writes) AS DECIMAL(10,1)) AS [# Write Pct],
CAST(100. * num_of_bytes_read/(num_of_bytes_read + num_of_bytes_written) AS DECIMAL(10,1)) AS [Read Bytes Pct],
CAST(100. * num_of_bytes_written/(num_of_bytes_read + num_of_bytes_written) AS DECIMAL(10,1)) AS [Written Bytes Pct]
FROM sys.dm_io_virtual_file_stats(DB_ID(), NULL) OPTION (RECOMPILE);

-- This helps you characterize your workload better from an I/O perspective for this database



-- Top cached queries by Execution Count (SQL Server 2008)  (Query 34)(Query Execution Count)
-- SQL Server 2008 R2 SP1 and greater only
SELECT TOP (250) qs.execution_count, qs.total_rows, qs.last_rows, qs.min_rows, qs.max_rows,
qs.last_elapsed_time, qs.min_elapsed_time, qs.max_elapsed_time,
total_worker_time, total_logical_reads, 
SUBSTRING(qt.TEXT,qs.statement_start_offset/2 +1,
(CASE WHEN qs.statement_end_offset = -1
			THEN LEN(CONVERT(NVARCHAR(MAX), qt.TEXT)) * 2
	  ELSE qs.statement_end_offset END - qs.statement_start_offset)/2) AS query_text 
FROM sys.dm_exec_query_stats AS qs WITH (NOLOCK)
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) AS qt
ORDER BY qs.execution_count DESC OPTION (RECOMPILE);

-- Uses several new rows returned columns to help troubleshoot performance problems


-- Top Cached SPs By Execution Count (SQL 2008) (SQL 2008 R2 SP1 only) (Query 35)(SP Execution Count)
SELECT TOP(250) p.name AS [SP Name], qs.execution_count,
ISNULL(qs.execution_count/DATEDIFF(Second, qs.cached_time, GETDATE()), 0) AS [Calls/Second],
qs.total_worker_time/qs.execution_count AS [AvgWorkerTime], qs.total_worker_time AS [TotalWorkerTime],  
qs.total_elapsed_time, qs.total_elapsed_time/qs.execution_count AS [avg_elapsed_time],
qs.cached_time
FROM sys.procedures AS p WITH (NOLOCK)
INNER JOIN sys.dm_exec_procedure_stats AS qs WITH (NOLOCK)
ON p.[object_id] = qs.[object_id]
WHERE qs.database_id = DB_ID()
ORDER BY qs.execution_count DESC OPTION (RECOMPILE);

-- Tells you which cached stored procedures are called the most often
-- This helps you characterize and baseline your workload


-- Top Cached SPs By Avg Elapsed Time (SQL 2008)  (Query 36)(SP Avg Elapsed Time)
SELECT TOP(25) p.name AS [SP Name], qs.total_elapsed_time/qs.execution_count AS [avg_elapsed_time], 
qs.total_elapsed_time, qs.execution_count, ISNULL(qs.execution_count/DATEDIFF(Second, qs.cached_time, 
GETDATE()), 0) AS [Calls/Second], qs.total_worker_time/qs.execution_count AS [AvgWorkerTime], 
qs.total_worker_time AS [TotalWorkerTime], qs.cached_time
FROM sys.procedures AS p WITH (NOLOCK)
INNER JOIN sys.dm_exec_procedure_stats AS qs WITH (NOLOCK)
ON p.[object_id] = qs.[object_id]
WHERE qs.database_id = DB_ID()
ORDER BY avg_elapsed_time DESC OPTION (RECOMPILE);

-- This helps you find long-running cached stored procedures that
-- may be easy to optimize with standard query tuning techniques


-- Top Cached SPs By Avg Elapsed Time with execution time variability   (Query 37)(SP Avg Elapsed Variation)
SELECT TOP(25) p.name AS [SP Name], qs.execution_count, qs.min_elapsed_time,
qs.total_elapsed_time/qs.execution_count AS [avg_elapsed_time],
qs.max_elapsed_time, qs.last_elapsed_time,  qs.cached_time
FROM sys.procedures AS p WITH (NOLOCK)
INNER JOIN sys.dm_exec_procedure_stats AS qs WITH (NOLOCK)
ON p.[object_id] = qs.[object_id]
WHERE qs.database_id = DB_ID()
ORDER BY avg_elapsed_time DESC OPTION (RECOMPILE);

-- This gives you some interesting information about the variability in the
-- execution time of your cached stored procedures, which is useful for tuning


-- Top Cached SPs By Total Worker time (SQL 2008). Worker time relates to CPU cost  (Query 38)(SP Worker Time)
SELECT TOP(25) p.name AS [SP Name], qs.total_worker_time AS [TotalWorkerTime], 
qs.total_worker_time/qs.execution_count AS [AvgWorkerTime], qs.execution_count, 
ISNULL(qs.execution_count/DATEDIFF(Second, qs.cached_time, GETDATE()), 0) AS [Calls/Second],
qs.total_elapsed_time, qs.total_elapsed_time/qs.execution_count 
AS [avg_elapsed_time], qs.cached_time
FROM sys.procedures AS p WITH (NOLOCK)
INNER JOIN sys.dm_exec_procedure_stats AS qs WITH (NOLOCK)
ON p.[object_id] = qs.[object_id]
WHERE qs.database_id = DB_ID()
ORDER BY qs.total_worker_time DESC OPTION (RECOMPILE);

-- This helps you find the most expensive cached stored procedures from a CPU perspective
-- You should look at this if you see signs of CPU pressure


-- Top Cached SPs By Total Logical Reads (SQL 2008). Logical reads relate to memory pressure  (Query 39)(SP Logical Reads)
SELECT TOP(25) p.name AS [SP Name], qs.total_logical_reads AS [TotalLogicalReads], 
qs.total_logical_reads/qs.execution_count AS [AvgLogicalReads],qs.execution_count, 
ISNULL(qs.execution_count/DATEDIFF(Second, qs.cached_time, GETDATE()), 0) AS [Calls/Second], 
qs.total_elapsed_time, qs.total_elapsed_time/qs.execution_count 
AS [avg_elapsed_time], qs.cached_time
FROM sys.procedures AS p WITH (NOLOCK)
INNER JOIN sys.dm_exec_procedure_stats AS qs WITH (NOLOCK)
ON p.[object_id] = qs.[object_id]
WHERE qs.database_id = DB_ID()
ORDER BY qs.total_logical_reads DESC OPTION (RECOMPILE);

-- This helps you find the most expensive cached stored procedures from a memory perspective
-- You should look at this if you see signs of memory pressure


-- Top Cached SPs By Total Physical Reads (SQL 2008). Physical reads relate to disk I/O pressure  (Query 40)(SP Physical Reads)
SELECT TOP(25) p.name AS [SP Name],qs.total_physical_reads AS [TotalPhysicalReads], 
qs.total_physical_reads/qs.execution_count AS [AvgPhysicalReads], qs.execution_count, 
qs.total_logical_reads,qs.total_elapsed_time, qs.total_elapsed_time/qs.execution_count 
AS [avg_elapsed_time], qs.cached_time 
FROM sys.procedures AS p WITH (NOLOCK)
INNER JOIN sys.dm_exec_procedure_stats AS qs WITH (NOLOCK)
ON p.[object_id] = qs.[object_id]
WHERE qs.database_id = DB_ID()
AND qs.total_physical_reads > 0
ORDER BY qs.total_physical_reads DESC, qs.total_logical_reads DESC OPTION (RECOMPILE);

-- This helps you find the most expensive cached stored procedures from a read I/O perspective
-- You should look at this if you see signs of I/O pressure or of memory pressure
       
-- Top Cached SPs By Total Logical Writes (SQL 2008)  (Query 41)(SP Logical Writes)
-- Logical writes relate to both memory and disk I/O pressure 
SELECT TOP(25) p.name AS [SP Name], qs.total_logical_writes AS [TotalLogicalWrites], 
qs.total_logical_writes/qs.execution_count AS [AvgLogicalWrites], qs.execution_count,
ISNULL(qs.execution_count/DATEDIFF(Second, qs.cached_time, GETDATE()), 0) AS [Calls/Second],
qs.total_elapsed_time, qs.total_elapsed_time/qs.execution_count AS [avg_elapsed_time], 
qs.cached_time
FROM sys.procedures AS p WITH (NOLOCK)
INNER JOIN sys.dm_exec_procedure_stats AS qs WITH (NOLOCK)
ON p.[object_id] = qs.[object_id]
WHERE qs.database_id = DB_ID()
ORDER BY qs.total_logical_writes DESC OPTION (RECOMPILE);

-- This helps you find the most expensive cached stored procedures from a write I/O perspective
-- You should look at this if you see signs of I/O pressure or of memory pressure


-- Lists the top statements by average input/output usage for the current database  (Query 42)(Top IO)
SELECT TOP(50) OBJECT_NAME(qt.objectid) AS [SP Name],
(qs.total_logical_reads + qs.total_logical_writes) /qs.execution_count AS [Avg IO],
SUBSTRING(qt.[text],qs.statement_start_offset/2, 
	(CASE 
		WHEN qs.statement_end_offset = -1 
	 THEN LEN(CONVERT(nvarchar(max), qt.[text])) * 2 
		ELSE qs.statement_end_offset 
	 END - qs.statement_start_offset)/2) AS [Query Text]	
FROM sys.dm_exec_query_stats AS qs WITH (NOLOCK)
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) AS qt
WHERE qt.[dbid] = DB_ID()
ORDER BY [Avg IO] DESC OPTION (RECOMPILE);

-- Helps you find the most expensive statements for I/O by SP



-- Possible Bad NC Indexes (writes > reads)  (Query 43)(Bad NC Indexes)
SELECT OBJECT_NAME(s.[object_id]) AS [Table Name], i.name AS [Index Name], i.index_id, i.is_disabled,
user_updates AS [Total Writes], user_seeks + user_scans + user_lookups AS [Total Reads],
user_updates - (user_seeks + user_scans + user_lookups) AS [Difference]
FROM sys.dm_db_index_usage_stats AS s WITH (NOLOCK)
INNER JOIN sys.indexes AS i WITH (NOLOCK)
ON s.[object_id] = i.[object_id]
AND i.index_id = s.index_id
WHERE OBJECTPROPERTY(s.[object_id],'IsUserTable') = 1
AND s.database_id = DB_ID()
AND user_updates > (user_seeks + user_scans + user_lookups)
AND i.index_id > 1
ORDER BY [Difference] DESC, [Total Writes] DESC, [Total Reads] ASC OPTION (RECOMPILE);

-- Look for indexes with high numbers of writes and zero or very low numbers of reads
-- Consider your complete workload
-- Investigate further before dropping an index!


-- Missing Indexes for current database by Index Advantage  (Query 44)(Missing Indexes)
SELECT user_seeks * avg_total_user_cost * (avg_user_impact * 0.01) AS [index_advantage], 
migs.last_user_seek, mid.[statement] AS [Database.Schema.Table],
mid.equality_columns, mid.inequality_columns, mid.included_columns,
migs.unique_compiles, migs.user_seeks, migs.avg_total_user_cost, migs.avg_user_impact
FROM sys.dm_db_missing_index_group_stats AS migs WITH (NOLOCK)
INNER JOIN sys.dm_db_missing_index_groups AS mig WITH (NOLOCK)
ON migs.group_handle = mig.index_group_handle
INNER JOIN sys.dm_db_missing_index_details AS mid WITH (NOLOCK)
ON mig.index_handle = mid.index_handle
WHERE mid.database_id = DB_ID() -- Remove this to see for entire instance
ORDER BY index_advantage DESC OPTION (RECOMPILE);

-- Look at index advantage, last user seek time, number of user seeks to help determine source and importance
-- SQL Server is overly eager to add included columns, so beware
-- Do not just blindly add indexes that show up from this query!!!


-- Find missing index warnings for cached plans in the current database  (Query 45)(Missing Index Warnings)
-- Note: This query could take some time on a busy instance
SELECT TOP(25) OBJECT_NAME(objectid) AS [ObjectName], 
               query_plan, cp.objtype, cp.usecounts
FROM sys.dm_exec_cached_plans AS cp WITH (NOLOCK)
CROSS APPLY sys.dm_exec_query_plan(cp.plan_handle) AS qp
WHERE CAST(query_plan AS NVARCHAR(MAX)) LIKE N'%MissingIndex%'
AND dbid = DB_ID()
ORDER BY cp.usecounts DESC OPTION (RECOMPILE);

-- Helps you connect missing indexes to specific stored procedures
-- This can help you decide whether to add them or not


-- Breaks down buffers used by current database by object (table, index) in the buffer cache  (Query 46)(Buffer Usage)
-- Note: This query could take some time on a busy instance
SELECT OBJECT_NAME(p.[object_id]) AS [ObjectName], 
p.index_id, COUNT(*)/128 AS [Buffer size(MB)],  COUNT(*) AS [BufferCount], 
p.data_compression_desc AS [CompressionType], a.type_desc, p.[rows]
FROM sys.allocation_units AS a WITH (NOLOCK)
INNER JOIN sys.dm_os_buffer_descriptors AS b WITH (NOLOCK)
ON a.allocation_unit_id = b.allocation_unit_id
INNER JOIN sys.partitions AS p WITH (NOLOCK)
ON a.container_id = p.partition_id
WHERE b.database_id = CONVERT(int,DB_ID())
AND p.[object_id] > 100
GROUP BY p.[object_id], p.index_id, p.data_compression_desc, a.type_desc, p.[rows]
ORDER BY [BufferCount] DESC OPTION (RECOMPILE);

-- Tells you what tables and indexes are using the most memory in the buffer cache


-- Get Table names, row counts, and compression status for clustered index or heap  (Query 47)(Table Sizes)
SELECT OBJECT_NAME(object_id) AS [ObjectName], 
SUM(Rows) AS [RowCount], data_compression_desc AS [CompressionType]
FROM sys.partitions WITH (NOLOCK)
WHERE index_id < 2 --ignore the partitions from the non-clustered index if any
AND OBJECT_NAME(object_id) NOT LIKE N'sys%'
AND OBJECT_NAME(object_id) NOT LIKE N'queue_%' 
AND OBJECT_NAME(object_id) NOT LIKE N'filestream_tombstone%' 
AND OBJECT_NAME(object_id) NOT LIKE N'fulltext%'
AND OBJECT_NAME(object_id) NOT LIKE N'ifts_comp_fragment%'
GROUP BY object_id, data_compression_desc
ORDER BY SUM(Rows) DESC OPTION (RECOMPILE);

-- Gives you an idea of table sizes, and possible data compression opportunities


-- When were Statistics last updated on all indexes?  (Query 48)(Statistics Update)
SELECT o.name, i.name AS [Index Name],  
      STATS_DATE(i.[object_id], i.index_id) AS [Statistics Date], 
      s.auto_created, s.no_recompute, s.user_created, st.row_count
FROM sys.objects AS o WITH (NOLOCK)
INNER JOIN sys.indexes AS i WITH (NOLOCK)
ON o.[object_id] = i.[object_id]
INNER JOIN sys.stats AS s WITH (NOLOCK)
ON i.[object_id] = s.[object_id] 
AND i.index_id = s.stats_id
INNER JOIN sys.dm_db_partition_stats AS st WITH (NOLOCK)
ON o.[object_id] = st.[object_id]
AND i.[index_id] = st.[index_id]
WHERE o.[type] = 'U'
ORDER BY STATS_DATE(i.[object_id], i.index_id) ASC OPTION (RECOMPILE);  

-- Helps discover possible problems with out-of-date statistics
-- Also gives you an idea which indexes are the most active


-- Get fragmentation info for all indexes above a certain size in the current database  (Query 49)(Index Fragmentation) 
-- Note: This could take some time on a very large database
SELECT DB_NAME(database_id) AS [Database Name], OBJECT_NAME(ps.OBJECT_ID) AS [Object Name], 
i.name AS [Index Name], ps.index_id, index_type_desc,
avg_fragmentation_in_percent, fragment_count, page_count
FROM sys.dm_db_index_physical_stats(DB_ID(),NULL, NULL, NULL ,N'LIMITED') AS ps 
INNER JOIN sys.indexes AS i WITH (NOLOCK)
ON ps.[object_id] = i.[object_id] 
AND ps.index_id = i.index_id
WHERE database_id = DB_ID()
AND page_count > 1500
ORDER BY avg_fragmentation_in_percent DESC OPTION (RECOMPILE);

-- Helps determine whether you have framentation in your relational indexes
-- and how effective your index maintenance strategy is


--- Index Read/Write stats (all tables in current DB) ordered by Reads  (Query 50)(Index Usage - Reads)
SELECT OBJECT_NAME(s.[object_id]) AS [ObjectName], i.name AS [IndexName], i.index_id,
	   user_seeks + user_scans + user_lookups AS [Reads], s.user_updates AS [Writes],  
	   i.type_desc AS [IndexType], i.fill_factor AS [FillFactor]
FROM sys.dm_db_index_usage_stats AS s WITH (NOLOCK)
INNER JOIN sys.indexes AS i WITH (NOLOCK)
ON s.[object_id] = i.[object_id]
WHERE OBJECTPROPERTY(s.[object_id],'IsUserTable') = 1
AND i.index_id = s.index_id
AND s.database_id = DB_ID()
ORDER BY user_seeks + user_scans + user_lookups DESC OPTION (RECOMPILE); -- Order by reads

-- Show which indexes in the current database are most active for Reads


--- Index Read/Write stats (all tables in current DB) ordered by Writes  (Query 51)(Index Usage - Writes)
SELECT OBJECT_NAME(s.[object_id]) AS [ObjectName], i.name AS [IndexName], i.index_id,
	   s.user_updates AS [Writes], user_seeks + user_scans + user_lookups AS [Reads], 
	   i.type_desc AS [IndexType], i.fill_factor AS [FillFactor]
FROM sys.dm_db_index_usage_stats AS s WITH (NOLOCK)
INNER JOIN sys.indexes AS i WITH (NOLOCK)
ON s.[object_id] = i.[object_id]
WHERE OBJECTPROPERTY(s.[object_id],'IsUserTable') = 1
AND i.index_id = s.index_id
AND s.database_id = DB_ID()
ORDER BY s.user_updates DESC OPTION (RECOMPILE);						 -- Order by writes

-- Show which indexes in the current database are most active for Writes


-- Look at recent Full backups for the current database (Query 52)(Recent Full Backups)
SELECT TOP (30) bs.server_name, bs.database_name AS [Database Name], 
CONVERT (BIGINT, bs.backup_size / 1048576 ) AS [Uncompressed Backup Size (MB)],
CONVERT (BIGINT, bs.compressed_backup_size / 1048576 ) AS [Compressed Backup Size (MB)],
CONVERT (NUMERIC (20,2), (CONVERT (FLOAT, bs.backup_size) /
CONVERT (FLOAT, bs.compressed_backup_size))) AS [Compression Ratio], 
DATEDIFF (SECOND, bs.backup_start_date, bs.backup_finish_date) AS [Backup Elapsed Time (sec)],
bs.backup_finish_date AS [Backup Finish Date]
FROM msdb.dbo.backupset AS bs WITH (NOLOCK)
WHERE DATEDIFF (SECOND, bs.backup_start_date, bs.backup_finish_date) > 0 
AND bs.backup_size > 0
AND bs.type = 'D' -- Change to L if you want Log backups
AND database_name = DB_NAME(DB_ID())
ORDER BY bs.backup_finish_date DESC OPTION (RECOMPILE);

-- Are your backup sizes and times changing over time?


-- Get the average full backup size by month for the current database (SQL 2008) (Query 53)(Size History)
-- This helps you understand your database growth over time
-- Adapted from Erin Stellato
SELECT [database_name] AS [Database], DATEPART(month,[backup_start_date]) AS [Month],
CAST(AVG([backup_size]/1024/1024) AS DECIMAL(15,2)) AS [Backup Size (MB)],
CAST(AVG([compressed_backup_size]/1024/1024) AS DECIMAL(15,2)) AS [Compressed Backup Size (MB)],
CAST(AVG([backup_size]/[compressed_backup_size]) AS DECIMAL(15,2)) AS [Compression Ratio]
FROM msdb.dbo.backupset WITH (NOLOCK)
WHERE [database_name] = DB_NAME(DB_ID())
AND [type] = 'D'
AND backup_start_date >= DATEADD(MONTH, -12, GETDATE())
GROUP BY [database_name],DATEPART(mm,[backup_start_date]) OPTION (RECOMPILE);

-- The Backup Size (MB) (without backup compression) shows the true size of your database over time
-- This helps you track and plan your data size growth
-- It is possible that your data files may be larger on disk due to empty space within those files