param(
    [int]$Year = 2015 
)

$template = Get-Content (Join-Path $PSScriptRoot "Template.txt") -Raw

$newDirectory = Join-Path $PSScriptRoot "Solutions" "Year$Year"

if(!(Test-Path $NewDirectory)){
    New-Item $newDirectory -ItemType Directory
}

for($i = 1; $i -le 25; $i++) {
    $newFile = Join-Path $newDirectory "Day$("{0:00}" -f $i)"  "Solution.cs"
    if(!(Test-Path $newFile)){
        New-Item $newFile -ItemType File -Value ($template -replace "<YEAR>", $Year -replace "<DAY>", "$("{0:00}" -f $i)") -Force | Out-Null
    }
}