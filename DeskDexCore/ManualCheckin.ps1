$adapter = Get-WmiObject win32_networkadapter | Where-Object {$_.Speed -gt 0} | select-object -first 1

if ($adapter) {
    $u = whoami
    $user = Get-WMIObject Win32_UserAccount | where caption -eq $u | select-object -first 1
    $checkin = @{
        address = $adapter.MACAddress
        acid = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name -replace ".*\\", ""
        display = $user.FullName
    }
    $json = $checkin | ConvertTo-Json
    Invoke-RestMethod -UseDefaultCredentials -Method 'Post' -Body $json -Uri "https://localhost:44356/api/checkin" -ContentType 'application/json'
}