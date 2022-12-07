param(
    [int]$Year = (Get-Date).Year
)

$template = @"
using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year<YEAR>
{
    [DayInfo(<DAY>, <YEAR>, `"`")]
    class Day<DAY> : ASolution
    {
        public Day<DAY>() : base()
        {

        }

        protected override object SolvePartOne()
        {
            return null;
        }

        protected override object SolvePartTwo()
        {
            return null;
        }
    }
}
"@

$newDirectory = Join-Path $PSScriptRoot ".." "Solutions" "Year$Year" 

if(!(Test-Path $newDirectory)) {
    New-Item $newDirectory -ItemType Directory | Out-Null
}

for($i = 1; $i -le 25; $i++) {
    $newFile = Join-Path $newDirectory "Day$("{0:00}" -f $i)-Solution.cs"  
    if(!(Test-Path $newFile)) {
        New-Item $newFile -ItemType File -Value ($template -replace "<YEAR>", $Year -replace "<DAY>", "$("{0:00}" -f $i)") -Force | Out-Null
    }
}

Write-Host "Files Generated"