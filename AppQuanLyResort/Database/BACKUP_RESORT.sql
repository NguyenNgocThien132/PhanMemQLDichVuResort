﻿--BACKUP
BACKUP DATABASE QLDVRESORT
TO DISK = '(Đường dẫn backup)\QLDVRESORT_backup.bak'
WITH FORMAT, 
     MEDIANAME = 'QLDVRESORT_Backup', 
     NAME = 'Full Backup of QLDVRESORT';

--BACKUP LOG
BACKUP LOG QLDVRESORT
TO DISK = '(Đường dẫn backup)\QLDVRESORT_LogBackup.trn'


--PHÂN CHIA TÀI NGUYÊN

ALTER DATABASE QLDVRESORT
ADD FILE
(
    NAME = N'QLDVRESORT_data_mdf', 
    FILENAME = N'(Đường dẫn lưu trử)\QLDVRESORT.mdf', 
    SIZE = 8192KB, 
    MAXSIZE = UNLIMITED, 
    FILEGROWTH = 65536KB
);

-- Thêm tệp dữ liệu phụ (.ndf)
ALTER DATABASE QLDVRESORT
ADD FILE
(
    NAME = N'QLDVRESORT_data_ndf',
    FILENAME = N'(Đường dẫn lưu trử)\QLDVRESORT.ndf', 
    SIZE = 8192KB, 
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 65536KB 
);


ALTER DATABASE QLDVRESORT
ADD FILEGROUP Group_Historyesort; 

ALTER DATABASE QLDVRESORT
ADD LOG FILE
(
    NAME = N'QLDVRESORT_data_log',
    FILENAME = N'(Đường dẫn lưu trử)\QLDVRESORT_log.ldf',
    SIZE = 8192KB,
    MAXSIZE = 2048GB,
    FILEGROWTH = 65536KB
)
TO FILEGROUP Group_Historyesort; 



