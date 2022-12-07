$regexString = "base\((?<info>\s*\d{2},\s*\d{4},\s*""[^\)]*"")\)"
$RegexMatcher = [Regex]::new($regexString)

foreach($year in Get-ChildItem $(Join-Path $PSScriptRoot ".." "Solutions") -Directory) {
    foreach($day in Get-ChildItem "$($year)/*.cs") {
        $content = Get-Content $day
        $matches = $RegexMatcher.matches($content)
        $info = $matches[0].Groups['info'].Value

        $content = $content.replace('class Day', "[DayInfo($($info))]`n    class Day")

        $content = $content -Replace $regexString, 'base()'
        Set-Content -Path $day -Value $content
    }
}