@echo off
cls

setlocal enabledelayedexpansion

REM Uncomment and set if you want to override Java path
SET JAVA_HOME="C:\Program Files\jdk-21"
SET PATH=%JAVA_HOME%\bin;%PATH%

REM Move to project root from docs folder
cd ..

REM Variables for paths
set ERRMSG=
set PRAC_BIN=bin
set PRAC_DOCS=docs
set PRAC_SRC=src

REM --- CLEAN ---
:CLEAN
echo ~~~ Cleaning project ~~~
if exist %PRAC_BIN% (
    del /s /q %PRAC_BIN%\*.class
)
if exist %PRAC_DOCS%\JavaDoc (
    rmdir /s /q %PRAC_DOCS%\JavaDoc
)
if errorlevel 1 (
    echo Warning: Cleanup encountered errors, continuing...
)

REM --- COMPILE ---
:COMPILE
echo ~~~ Compiling project ~~~
javac -sourcepath %PRAC_SRC% -d %PRAC_BIN% %PRAC_SRC%\NotificationSystemDemo.java
if errorlevel 1 (
    set ERRMSG=Error compiling project.
    goto ERROR
)

REM --- RUN ---
:RUN
echo ~~~ Running project ~~~
java -cp %PRAC_BIN% NotificationSystemDemo
if errorlevel 1 (
    set ERRMSG=Error running project.
    goto ERROR
)

REM --- ERROR HANDLING ---
:ERROR
echo %ERRMSG%

REM --- END ---
:END
echo ~~~ Process complete ~~~
pause
