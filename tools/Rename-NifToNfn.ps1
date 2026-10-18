[CmdletBinding()]
param([switch] $Apply)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$workspaceRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$nativeRoot = Join-Path $workspaceRoot 'src/Nitload.Native'
$textRoots = @(
    (Join-Path $workspaceRoot 'Directory.Build.targets'),
    $nativeRoot,
    (Join-Path $workspaceRoot 'src/Nitload.Common'),
    (Join-Path $workspaceRoot 'src/Nitload.Mathematics'),
    (Join-Path $workspaceRoot 'src/Nitload.Graphics')
)
$textExtensions = @('.c', '.h', '.cs', '.cmake', '.txt', '.md', '.vcxproj', '.filters', '.targets')
$utf8 = [Text.UTF8Encoding]::new($false, $true)

$files = foreach ($root in $textRoots) {
    if (Test-Path -LiteralPath $root -PathType Leaf) {
        Get-Item -LiteralPath $root
    } else {
        Get-ChildItem -LiteralPath $root -Recurse -File | Where-Object {
            $_.Extension -in $textExtensions -and
            $_.FullName -notmatch '[\\/](bin|obj|cmake_build)[\\/]'
        }
    }
}

$changes = [Collections.Generic.List[string]]::new()
foreach ($file in $files) {
    $bytes = [IO.File]::ReadAllBytes($file.FullName)
    try { $content = $utf8.GetString($bytes) }
    catch [Text.DecoderFallbackException] { continue }
    if ($file.FullName -eq (Join-Path $nativeRoot 'README.md') -and
        $content.Contains('The C implementation uses `nfn`')) { continue }

    $updated = $content.Replace('NIF_', 'NFN_').Replace('nif_', 'nfn_').Replace('nif-', 'nfn-')
    $updated = $updated.Replace('NF_', 'NFN_').Replace('nf_', 'nfn_').Replace('nf-', 'nfn-')
    # The previous migration left private SN macros in this header.
    if ($file.Name -in @('nif_intrinsic.std.h', 'nf_intrinsic.std.h')) {
        $updated = $updated.Replace('_SN_', '_NFN_')
    }
    if ($updated -ceq $content) { continue }

    $changes.Add([IO.Path]::GetRelativePath($workspaceRoot, $file.FullName))
    if ($Apply) { [IO.File]::WriteAllText($file.FullName, $updated, $utf8) }
}

$moves = [Collections.Generic.List[object]]::new()
foreach ($directory in @('nif-head', 'nif-src', 'nif-template', 'nf-head', 'nf-src', 'nf-template')) {
    $sourceDir = Join-Path $nativeRoot $directory
    if (-not (Test-Path -LiteralPath $sourceDir)) { continue }
    foreach ($file in Get-ChildItem -LiteralPath $sourceDir -File | Where-Object { $_.Name -cmatch '^(nif|nf)_' }) {
        $moves.Add(@($file.FullName, (Join-Path $sourceDir ($file.Name -creplace '^(nif|nf)_', 'nfn_'))))
    }
    $moves.Add(@($sourceDir, (Join-Path $nativeRoot ($directory -creplace '^(nif|nf)-', 'nfn-'))))
}

foreach ($move in $moves) {
    if (Test-Path -LiteralPath $move[1]) { throw "Rename target already exists: $($move[1])" }
}

"Text files to update: $($changes.Count)"
$changes | ForEach-Object { "  TEXT $_" }
"Paths to rename: $($moves.Count)"
$moves | ForEach-Object {
    "  MOVE $([IO.Path]::GetRelativePath($workspaceRoot, $_[0])) -> $([IO.Path]::GetRelativePath($workspaceRoot, $_[1]))"
}

if (-not $Apply) {
    'Dry run only. Re-run with -Apply to perform the migration.'
    exit 0
}

foreach ($move in $moves) { Move-Item -LiteralPath $move[0] -Destination $move[1] }
'NFN prefix migration completed.'
