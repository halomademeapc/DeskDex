$adapter = Get-WmiObject win32_networkadapter | Where-Object {$_.Name -like "Realtek USB GbE Family Controller" -and $_.Speed -gt 0} | select-object -first 1

if ($adapter) {
    $checkin = @{
        address = $adapter.MACAddress
        acid    = $env:username
        display = ([adsi]"WinNT://$env:userdomain/$env:username").FullName.ToString()
    }
    $json = $checkin | ConvertTo-Json
    Invoke-RestMethod -UseDefaultCredentials -Method 'Post' -Body $json -Uri "https://deskdex.azurewebsites.net/api/checkin" -ContentType 'application/json'
}