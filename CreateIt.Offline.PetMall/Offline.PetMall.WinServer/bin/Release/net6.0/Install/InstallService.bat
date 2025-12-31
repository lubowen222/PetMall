:: 安装命令
:: sc <server> [command] [service name] <option1> <option2>...


@echo off
echo --------------------------------------
echo    Windows服务，开始安装
echo --------------------------------------

sc create SpiderWinServer binPath= "D:\work\web\winserver\server\Offline.Sup.Spider.WinServer.exe" start= auto DisplayName= "SpiderWinServer"
sc description SpiderWinServer "官网爬虫服务"
pause