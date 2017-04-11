# CottonHarvestFileDownload
## Application Overview
The Cotton Harvest File Download Utility may be used to automate the download of Harvest ID files from MyJohnDeere.  

## Download process 
The application download process will download all harvest id zip files, extract the files, and find the Harvest ID file in the archive.    For each harvest id file an output file is created.  Output files are named using the Machine ID and the timestamp that the file was downloaded.    All output files are written to a single folder selected by the user.    
The application maintains a row count for each unique file downloaded.   This is used on subsequent downloads to only output newly appended records.     The application identifies a file by the first data line instead of the filename, it is possible for the same file data to go into MyJohnDeere under different file names.   Therefore, matching on the first data line is a more accurate way of matching datasets.

## System Pre-requisites and Recommendations
* PC running Microsoft® Windows® version 7, 8.1, or 10
* At least 100MB storage space to install the application.   More space will be needed to store downloaded files.  Total storage required will depend on the number of files being received.
* Access to an active MyJohnDeere account and appropriate subscriptions and/or licenses to receive files from trusted partners.
* Microsoft .NET Framework 4.6.1.  If it does not already exist on your system, the setup installer will acquire and install this package for you.
* Microsoft SQL Express Local DB 2014 -  This is lightweight and free database engine from Microsoft.   If it does not already exist on your system, the setup installer will acquire and install this package for you.

