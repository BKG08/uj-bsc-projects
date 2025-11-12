REM This batch file is setup to run the UDPClient component of the project.
@echo off
cls

echo This batch file assumes UDPClient.java is located in ../src
echo Compiled classes will be placed in ../bin
echo.

setlocal enabledelayedexpansion

REM --- Paths for JDK ---
set "JAVA_HOME=C:\Program Files\jdk-21"
set "PATH=%JAVA_HOME%\bin;%PATH%"

REM --- Paths for JavaFX ---
set "USE_JAVAFX=true"
set "JAVAFX_HOME=C:\Program Files\javafx-sdk-21"
set "JAVAFX_MODULES=javafx.base,javafx.controls,javafx.fxml,javafx.graphics,javafx.media"
set "JAVAFX_ARGS="
if "%USE_JAVAFX%"=="true" (
    set "JAVAFX_ARGS=--module-path "%JAVAFX_HOME%\lib" --add-modules=%JAVAFX_MODULES%"
)

REM Variable for error messages
set ERRMSG=

REM Navigate to project root
cd ..

REM Create bin folder if it does not exist
if not exist bin mkdir bin

REM Compile UDPClient.java from src folder into bin folder
echo ~~~ Compiling UDPClient.java ~~~
javac %JAVAFX_ARGS% -d bin src\UDPClient.java

if %ERRORLEVEL% neq 0 (
    set ERRMSG=~~~! Error compiling UDPClient.java !~~~
    goto ERROR
)

REM Run the UDPClient from the bin folder
:RUN_CLIENT
echo ~~~ Running UDPClient Application ~~~
java %JAVAFX_ARGS% -cp bin UDPClient

if %ERRORLEVEL% neq 0 (
    set ERRMSG=~~~! Error running UDPClient !~~~
    goto ERROR
)

goto END

REM Something went wrong, display error.
:ERROR
echo ~~~! Fatal error with project !~~~
echo %ERRMSG%
pause

:END
echo ~~~ End ~~~
pause
