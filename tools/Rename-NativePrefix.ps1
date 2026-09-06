[CmdletBinding()]
param(
    [switch] $Apply
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$taskWorkspaceRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$nativeProjectRoot = Join-Path $taskWorkspaceRoot 'src\Nitload.Native'
$textRoots = @(
    $nativeProjectRoot
    (Join-Path $taskWorkspaceRoot 'src\Nitload.Common')
    (Join-Path $taskWorkspaceRoot 'src\Nitload.Mathematics')
    (Join-Path $taskWorkspaceRoot 'src\Nitload.Graphics')
)

$textExtensions = [Collections.Generic.HashSet[string]]::new(
    [StringComparer]::OrdinalIgnoreCase
)
@(
    '.c', '.h', '.inl', '.inc', '.ipp',
    '.cs', '.cmake', '.txt', '.md',
    '.csproj', '.vcxproj', '.filters',
    '.json', '.props', '.targets'
) | ForEach-Object { [void] $textExtensions.Add($_) }

$replacementRules = @(
    @{ Pattern = '\bSTGSHARP_'; Replacement = 'NIF_' }
    @{ Pattern = '\bSN_'; Replacement = 'NIF_' }
    @{ Pattern = '(?<![A-Za-z0-9])sn_'; Replacement = 'nif_' }
    @{ Pattern = '\bload_glfw_functions\b'; Replacement = 'nif_load_glfw_functions' }
    @{ Pattern = '\bload_intrinsic_function\b'; Replacement = 'nif_load_intrinsic_function' }
    @{ Pattern = '\bStgSharpNative\b'; Replacement = 'NitloadFrameWork' }
    @{ Pattern = '\bNitloadInnovationFramework\b'; Replacement = 'NitloadFrameWork' }
    @{ Pattern = '\bNitloadFramework\b'; Replacement = 'NitloadFrameWork' }
    @{ Pattern = '\bSNative\b'; Replacement = 'NIF_NATIVE_H' }
    @{ Pattern = 'sn-head'; Replacement = 'nif-head' }
    @{ Pattern = 'sn-src'; Replacement = 'nif-src' }
    @{ Pattern = 'sn-template'; Replacement = 'nif-template' }
)

function Test-TaskPath {
    param([Parameter(Mandatory)][string] $Path)

    $resolvedPath = [IO.Path]::GetFullPath($Path)
    if (-not $resolvedPath.StartsWith(
        $taskWorkspaceRoot + [IO.Path]::DirectorySeparatorChar,
        [StringComparison]::OrdinalIgnoreCase
    )) {
        throw "Path escapes workspace: $resolvedPath"
    }
}

function Get-TaskTextFile {
    foreach ($textRoot in $textRoots) {
        Get-ChildItem -LiteralPath $textRoot -Recurse -File | Where-Object {
            $relativePath = [IO.Path]::GetRelativePath($taskWorkspaceRoot, $_.FullName)
            if ($relativePath -match '(^|[\\/])(bin|obj|\.discard)([\\/]|$)') {
                return $false
            }

            $nativeLibRoot = Join-Path $nativeProjectRoot 'lib'
            if ($_.FullName.StartsWith(
                $nativeLibRoot + [IO.Path]::DirectorySeparatorChar,
                [StringComparison]::OrdinalIgnoreCase
            ) -and $_.Name -ne 'CMakeLists.txt') {
                return $false
            }

            return $textExtensions.Contains($_.Extension)
        }
    }
}

function Read-TaskText {
    param([Parameter(Mandatory)][string] $Path)

    $bytes = [IO.File]::ReadAllBytes($Path)
    $encoding = [Text.UTF8Encoding]::new($false, $true)
    $preambleLength = 0
    $preambleBytes = [byte[]]::new(0)

    if ($bytes.Length -ge 3 -and
        $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF) {
        $preambleLength = 3
    }
    elseif ($bytes.Length -ge 2 -and $bytes[0] -eq 0xFF -and $bytes[1] -eq 0xFE) {
        $encoding = [Text.UnicodeEncoding]::new($false, $false, $true)
        $preambleLength = 2
    }
    elseif ($bytes.Length -ge 2 -and $bytes[0] -eq 0xFE -and $bytes[1] -eq 0xFF) {
        $encoding = [Text.UnicodeEncoding]::new($true, $false, $true)
        $preambleLength = 2
    }

    if ($preambleLength -ne 0) {
        $preambleBytes = [byte[]] $bytes[0..($preambleLength - 1)]
    }

    [PSCustomObject]@{
        Text = $encoding.GetString($bytes, $preambleLength, $bytes.Length - $preambleLength)
        Encoding = $encoding
        Preamble = $preambleBytes
    }
}

function Write-TaskText {
    param(
        [Parameter(Mandatory)][string] $Path,
        [Parameter(Mandatory)][string] $Text,
        [Parameter(Mandatory)] $Encoding,
        [Parameter(Mandatory)][AllowEmptyCollection()][byte[]] $Preamble
    )

    $body = $Encoding.GetBytes($Text)
    $output = [byte[]]::new($Preamble.Length + $body.Length)
    if ($Preamble.Length -ne 0) {
        [Buffer]::BlockCopy($Preamble, 0, $output, 0, $Preamble.Length)
    }
    [Buffer]::BlockCopy($body, 0, $output, $Preamble.Length, $body.Length)
    [IO.File]::WriteAllBytes($Path, $output)
}

$changedTextFiles = [Collections.Generic.List[string]]::new()
foreach ($textFile in Get-TaskTextFile) {
    $fileContent = Read-TaskText -Path $textFile.FullName
    $updatedText = $fileContent.Text
    foreach ($rule in $replacementRules) {
        $updatedText = [Text.RegularExpressions.Regex]::Replace(
            $updatedText,
            $rule.Pattern,
            $rule.Replacement,
            [Text.RegularExpressions.RegexOptions]::CultureInvariant
        )
    }

    if ($updatedText -ceq $fileContent.Text) {
        continue
    }

    $relativePath = [IO.Path]::GetRelativePath($taskWorkspaceRoot, $textFile.FullName)
    $changedTextFiles.Add($relativePath)
    if ($Apply) {
        Write-TaskText -Path $textFile.FullName `
                       -Text $updatedText `
                       -Encoding $fileContent.Encoding `
                       -Preamble $fileContent.Preamble
    }
}

$pathMoves = [Collections.Generic.List[object]]::new()
Get-ChildItem -LiteralPath $nativeProjectRoot -Recurse -File |
    Where-Object {
        $_.Name -match '^sn_' -or
        $_.Name -cin @(
            'StgSharpNative.h',
            'NitloadInnovationFramework.h',
            'NitloadFramework.h'
        )
    } |
    ForEach-Object {
        $newName = if ($_.Name -cin @(
            'StgSharpNative.h',
            'NitloadInnovationFramework.h',
            'NitloadFramework.h'
        )) {
            'NitloadFrameWork.h'
        }
        else {
            $_.Name -replace '^sn_', 'nif_'
        }
        $pathMoves.Add([PSCustomObject]@{
            Source = $_.FullName
            Target = Join-Path $_.DirectoryName $newName
        })
    }

Get-ChildItem -LiteralPath $nativeProjectRoot -Recurse -Directory |
    Where-Object { $_.Name -in @('sn-head', 'sn-src', 'sn-template') } |
    Sort-Object { $_.FullName.Length } -Descending |
    ForEach-Object {
        $pathMoves.Add([PSCustomObject]@{
            Source = $_.FullName
            Target = Join-Path $_.Parent.FullName ($_.Name -replace '^sn-', 'nif-')
        })
    }

foreach ($pathMove in $pathMoves) {
    Test-TaskPath -Path $pathMove.Source
    Test-TaskPath -Path $pathMove.Target
    if (Test-Path -LiteralPath $pathMove.Target) {
        throw "Rename target already exists: $($pathMove.Target)"
    }
}

"Text files to update: $($changedTextFiles.Count)"
$changedTextFiles | ForEach-Object { "  TEXT $_" }
"Paths to rename: $($pathMoves.Count)"
$pathMoves | ForEach-Object {
    $source = [IO.Path]::GetRelativePath($taskWorkspaceRoot, $_.Source)
    $target = [IO.Path]::GetRelativePath($taskWorkspaceRoot, $_.Target)
    "  MOVE $source -> $target"
}

if (-not $Apply) {
    'Dry run only. Re-run with -Apply to perform the migration.'
    exit 0
}

foreach ($pathMove in $pathMoves) {
    Move-Item -LiteralPath $pathMove.Source -Destination $pathMove.Target
}

'NIF prefix migration completed.'
