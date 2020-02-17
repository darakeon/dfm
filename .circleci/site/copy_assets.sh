cp -r ~/.nuget/packages/bootstrap.less/3.4.1/content/Content/fonts site/MVC/Assets/Libs

mkdir site/MVC/Assets/Libs/Scripts
cp ~/.nuget/packages/bootstrap.less/3.4.1/content/Scripts/bootstrap.min.js site/MVC/Assets/Libs/Scripts/bootstrap.min.js
cp ~/.nuget/packages/jquery/3.4.1/Tools/jquery-3.4.1.intellisense.js site/MVC/Assets/Libs/Scripts/jquery.intellisense.js
cp ~/.nuget/packages/jquery/3.4.1/Content/Scripts/jquery-3.4.1.min.js site/MVC/Assets/Libs/Scripts/jquery.min.js
cp ~/.nuget/packages/jquery.moneymask/1.0.3.1/content/Scripts/jquery.moneymask.js site/MVC/Assets/Libs/Scripts/jquery.moneyMask.js
cp ~/.nuget/packages/jquery-qrcode/1.0.0/content/Scripts/jquery.qrcode.min.js site/MVC/Assets/Libs/Scripts/jquery.qrcode.min.js
cp ~/.nuget/packages/jquery-qrcode/1.0.0/content/Scripts/qrcode.min.js site/MVC/Assets/Libs/Scripts/qrcode.min.js
cp ~/.nuget/packages/microsoft.jquery.unobtrusive.ajax/3.2.6/Content/Scripts/jquery.unobtrusive-ajax.min.js site/MVC/Assets/Libs/Scripts/jquery.unobtrusive-ajax.min.js

echo "All assets copied"
