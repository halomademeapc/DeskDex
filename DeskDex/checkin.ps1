$adapter = Get-WmiObject win32_networkadapter | Where-Object {$_.Name -like "Realtek USB GbE Family Controller" -and $_.Speed -gt 0}

if ($adapter) {
    $checkin = @{
        address = $adapter.MACAddress
        acid = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
    }
    $json = $checkin | ConvertTo-Json
    Invoke-RestMethod -UseDefaultCredentials -Method 'Post' -Body $json -Uri "http://localhost:50580/api/checkin" -ContentType 'application/json'
}