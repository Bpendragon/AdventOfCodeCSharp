foreach($year in Get-ChildItem $(Join-Path $PSScriptRoot ".." "Solutions") -Directory) {
    foreach($day in Get-ChildItem $year -Directory) {
        foreach($item in Get-ChildItem $day) {
            Move-Item $item -Destination $($(Join-Path $item.Directory ".." "$($day.PSChildName)-$($item.PSChildName)"))
        }
    }
}