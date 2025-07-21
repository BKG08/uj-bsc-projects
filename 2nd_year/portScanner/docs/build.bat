@echo off
cls

REM === UJ CSC2B Build Script: Java + JavaFX ===

setlocal enabledelayedexpansion

REM === Configure JDK ===
set "JAVA_HOME=C:\Program Files\jdk-21"
set "PATH=%JAVA_HOME%\bin;%PATH%"

REM === Configure JavaFX ===
set "USE_JAVAFX=true"
set "JAVAFX_HOME=C:\Program Files\javafx-sdk-21"
set "JAVAFX_MODULES=javafx.base,javafx.controls,javafx.fxml,javafx.graphics,javafx.media"

if "%USE_JAVAFX%"=="true" (
    set "JAVAFX_ARGS_ENABLED=true"
) else (
    set "JAVAFX_ARGS_ENABLED=false"
)

echo JavaFX Modules: %JAVAFX_MODULES%
echo.

REM === Check Java Versions ===
javac -version
if ERRORLEVEL 1 (
    echo Error: javac not found or JDK misconfigured.
    pause
    exit /b
)

java -version
if ERRORLEVEL 1 (
    echo Error: java not found or JDK misconfigured.
    pause
    exit /b
)

pause

REM === Move to project root (one up from docs) ===
cd ..
if ERRORLEVEL 1 (
    echo Error: Could not navigate to project root.
    pause
    exit /b
)

REM === Set project folders ===
set "PRAC_BIN=.\bin"
set "PRAC_DOCS=.\docs"
set "PRAC_JDOC=JavaDoc"
set "PRAC_LIB=.\lib\*"
set "PRAC_SRC=.\src"

REM === Clean previous builds ===
del /s /q "%PRAC_BIN%\*.class" 2>nul
rmdir /s /q "%PRAC_DOCS%\%PRAC_JDOC%" 2>nul

REM === Compile ===
echo Compiling project...
if "%JAVAFX_ARGS_ENABLED%"=="true" (
    javac --module-path "%JAVAFX_HOME%\lib" --add-modules %JAVAFX_MODULES% -d "%PRAC_BIN%" -cp "%PRAC_BIN%;%PRAC_LIB%" "%PRAC_SRC%\Main.java"
) else (
    javac -d "%PRAC_BIN%" -cp "%PRAC_BIN%;%PRAC_LIB%" "%PRAC_SRC%\Main.java"
)
if ERRORLEVEL 1 (
    echo Error: Compilation failed.
    pause
    exit /b
)

REM === JavaDoc ===
echo Generating JavaDoc...
if "%JAVAFX_ARGS_ENABLED%"=="true" (
    javadoc --module-path "%JAVAFX_HOME%\lib" --add-modules %JAVAFX_MODULES% -d "%PRAC_DOCS%\%PRAC_JDOC%" -sourcepath "%PRAC_SRC%" -classpath "%PRAC_BIN%;%PRAC_LIB%" -author -version -use "%PRAC_SRC%\Main.java"
) else (
    javadoc -d "%PRAC_DOCS%\%PRAC_JDOC%" -sourcepath "%PRAC_SRC%" -classpath "%PRAC_BIN%;%PRAC_LIB%" -author -version -use "%PRAC_SRC%\Main.java"
)
if ERRORLEVEL 1 (
    echo Warning: JavaDoc generation may have encountered issues.
)

REM === Run ===
echo Running project...
if "%JAVAFX_ARGS_ENABLED%"=="true" (
    java --module-path "%JAVAFX_HOME%\lib" --add-modules %JAVAFX_MODULES% -cp "%PRAC_BIN%;%PRAC_LIB%" Main
) else (
    java -cp "%PRAC_BIN%;%PRAC_LIB%" Main
)

echo.
echo Build and run completed successfully!
pause
