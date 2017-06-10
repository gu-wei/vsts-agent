# VSTS YAML simplified script syntax

The goal of this document is to define a simplified syntax for running scripts and command lines. Running a script should not require the formal declaration `- task: name@version, inputs...`.

## Proposed well-known tasks

- `command`
- `bash`
- `powershell`

## `command`

```yaml
- command: full command line or inline script goes here
  ignoreExitCode: false
  failOnStderr: false
  script: false
  env:
    name1: value1
    name2: value2
```

`command` runs command lines using cmd.exe on Windows, and bash on Linux.

`ignoreExitCode` defaults to `false`.

`failOnStderr` defaults to `false`.

`script` affects Windows only. Defaults to `false`. Must be set to `true` to enable multiple lines
on Windows. When `true`, the command is embedded in a .cmd file and subject to different interpreter
rules. Otherwise cmd.exe is used to execute the command directly. For more details, see FWLINK.

### Works on all OS

It is crucial that we have a well-known task (i.e. `command`) that works on both Windows and Linux.
Many popular CI tools work on Windows and Linux. For example: git, node, npm, tfx.

### Generate a temp script, or not?

On Linux we will always generate a temp script to run the command.

On Windows we cannot blindly generate a temp script to run the command. The problem is `%` is interpreted
differently on the command line versus within a script. The script interpreter will interfere with URLs
that contain encoded characters. In an interactive shell non-existing variables are not replaced. In a
script non-existing variables are replaced with empty.

Example 1:
<br />In an interactive shell, `echo hello%20world` outputs `hello%20world`.
<br />In a script, `echo hello%20world` outputs `hello0world` (assuming arg 2 is not specified).

Example 2:
<br />In an interactive shell, `echo hello %nosuch% var` outputs `hello %nosuch% var`.
<br />In a script, `echo hello %nosuch% var` outputs `hello  var`.

### cmd.exe command line options

Wrap command with `"<FULL_PATH_TO_CMD.EXE>" /Q /D /E:ON /V:OFF /S /C "<COMMAND>"`
- `/Q` Turns echo off.
  - Not sure whether we should set /Q.
    <br />
    <br />
    Reasons to set:
    <br />A) Consistency with Linux.
    <br />B) Folks commonly turn off echo in their own scripts. More often than not?
    <br />
    <br />
    Reasons to not set:
    <br />A) When generating a temp script, script contents will not otherwise get traced to the build output log.
    <br />
    <br />
    Another option would be to set /Q and the handler can dump the script contents to the build output log. Yet another option would be to dump to build debug log.
- `/D` Disable execution of AutoRun commands from registry
  - The motivation is to prevent accidental interference. Disabling autorun commands should
    be OK since it doesn't make sense for a CI build to depend on the presence of auto-run commands (bizarre coupling).
- `/E:ON` Enable command extensions
  - Command extensions are enabled by default, unless disabled via registry.
- `/V:OFF` Disable delayed environment expansion.
  - Delayed environment expansion is disabled by default, unless enabled via registry.
- `/S` will cause first and last quote after /C to be stripped

### bash command line options

TODO

### Other considerations

- For scenario when generating a script on Windows, revisit details from previous conversation with Philip regarding
  bubbling error level. IIRC the feedback was to consider CALL and checking %ERRORLEVEL% at the end if we generate a script.
  This has to do with error level from nested calls not bubbling as exit code of the process. Which is inconsitent
  wrt exit codes from externals processes. Note, this also could be related to the difference between .cmd and .bat files... investigate.
- Do we need a way to specify script should create a .bat file instead of .cmd? Seems unlikely. However, could be accomplished
  by allowing script:bat/cmd instead of simply true/false.
- Do we need a way to allow the user to influence the command line args to cmd.exe? For instance, to set one of the
  encoding switches:
  <br />
  `/A` Causes the output of internal commands to a pipe or file to be ANSI
  <br />
  `/U` Causes the output of internal commands to a pipe or file to be Unicode
  <br />
  <br />
  Otherwise we could consider a special property for these particular settings.
- Need option to invoke `dash` on Linux? Or always prefer `dash`? It seems unlikely that the small increase in startup
  perf would matter for our scenario.
- Consider a property `runInShell` (defaults to true) to offer a simple way out of shell escaping challenges. Motivation
  is similar to `verbatim` functionality in task lib. Furthermore, would that mean Linux users should be able to specify argv rather
  than forced to specify the full line?
- Do we need a way to influence how the agent interprets stdout encoding?

## `bash`

```yaml
- bash: inline script
  ignoreExitCode: false
  failOnStderr: false
```

`bash` runs inline script using bash from the PATH

`ignoreExitCode` defaults to `false`.

`failOnStderr` defaults to `false`.

### Notes

Always gens a script in agent.tempdirectory.

Specify noprofile, etc...

Works on Windows too if bash is in the PATH. Check other well-known locations for sh.exe?

## `powershell`

```yaml
- ps: inline script
  errorActionPreference: stop
  ignoreExitCode: false
  failOnStderr: false
```

`powershell` runs inline script using powershell from the PATH or well-known location.

`errorActionPreference` defaults to `stop`.

`ignoreExitCode` defaults to `false`.

`ignoreLASTEXITCODE` defaults to `false`.

`failOnStderr` defaults to `false`.

### Notes

- Try PATH, fallback to full desktop.
- Always gens a script in agent.tempdirectory.
- Specify noprofile, etc...
- Should we add `ignoreLASTEXITCODE: false`?
  <br />
  <br />
  Reasons to add:
  <br />A) Consider scenario where the customer runs msbuild and it returns 1. Doesn't bubble as it naturally does in cmd.exe.
  <br />
  <br />
  Reasons to not add:
  <br />A) Different from powershell behavior.
  <br />B) Exit or return or break or continue are
  strange... can prevent checking $LASTEXITCODE... investigate.
