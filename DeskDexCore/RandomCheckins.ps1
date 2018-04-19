$macs = "56:4b:39:1a:98:e5", "ff:2b:c5:7d:00:f8", "1a:16:22:47:99:3c", "24:6b:d5:0c:43:70", "8f:d8:fd:00:71:69", "4b:0d:43:96:8f:15", "42:18:59:8d:86:57", "35:7a:7e:3b:93:8d", "71:3f:04:45:65:db", "e6:86:cd:df:35:9a", "3c:5c:fd:96:ec:5e", "09:37:f1:2d:7b:43", "81:2b:6b:59:7a:56", "7e:d5:a2:4b:c7:2b", "84:8f:fc:28:e4:34", "f9:e6:33:db:2a:0c", "c4:78:c9:34:ec:4d", "86:56:b9:ba:29:15", "ce:0e:81:b4:52:d1", "c0:6a:ca:75:0d:bb", "66:68:f7:dc:5e:ad", "f5:14:8e:64:fc:eb", "65:06:2c:28:28:02", "fe:e4:c8:1b:e2:f6", "15:a0:18:b7:dc:e0", "80:8a:89:c7:7a:b3", "3c:e1:ed:7f:31:f4", "9b:c6:7a:6f:af:92", "86:9f:04:23:00:4e", "db:e0:cd:55:81:cf", "b2:e5:18:79:e0:8e", "4a:24:a6:28:20:8e", "ed:46:e0:69:24:e4", "c5:da:56:99:0b:db", "10:33:0b:2e:96:57", "6b:18:ff:cc:5a:6a", "0f:40:a1:bc:1e:f2", "a1:03:75:ad:0b:60", "b2:c0:c2:ac:2e:dc", "08:81:78:31:94:41", "9e:ea:a4:a5:76:53", "8f:0a:ed:7e:5f:fc", "73:d1:71:20:00:aa", "54:fc:a4:f3:3f:d1", "c4:f2:4f:eb:2c:c9", "de:9d:f4:74:86:03", "af:6e:f3:fb:8f:dc", "ec:70:84:95:22:31", "6a:5f:15:2b:58:94", "b9:f3:8d:f6:97:61"
$users = ("Taylor Akers", "usr0463"), ("Allyson Shockley", "usr0655"), ("Diana Wagner", "usr0617"), ("Autumn Dempsey", "usr0442"), ("Makayla Milligan", "usr0743"), ("Amaya Windham", "usr0351"), ("Hanna Marsh", "usr0455"), ("Alice Overton", "usr0425"), ("Stella Gruber", "usr0935"), ("Holly Rea", "usr0901"), ("Jasmin Lott", "usr0498"), ("Anna Smalley", "usr0745"), ("Rachel Henson", "usr0593"), ("Isabella Gilliland", "usr0872"), ("Talia Carlisle", "usr0476"), ("Isabella Erwin", "usr0990"), ("Savannah Gabriel", "usr0724"), ("Lily Carlson", "usr0510"), ("Kaelyn Colburn", "usr0726"), ("Hazel Worthington", "usr0600"), ("Ariana Coffey", "usr0183"), ("Jocelyn Fisk", "usr0516"), ("Kylie Loomis", "usr0371"), ("Esmeralda Dotson", "usr0273"), ("Katelyn Herbert", "usr0735"), ("Jamie Montalvo", "usr0893"), ("Luna Stanton", "usr0455"), ("Aaliyah Waggoner", "usr0399"), ("Laura Ring", "usr0284"), ("Destiny Pulliam", "usr0027")

$users | ForEach-Object {
    $checkin = @{
        address = $macs[(Get-Random -Maximum ([array]$macs).count)]
        acid    = $_[1]
        display = $_[0]
    }
    $json = $checkin | ConvertTo-Json
    Write-Host($json);
    Invoke-RestMethod -UseDefaultCredentials -Method 'Post' -Body $json -Uri "https://deskdex.azurewebsites.net/api/checkin" -ContentType 'application/json'
}