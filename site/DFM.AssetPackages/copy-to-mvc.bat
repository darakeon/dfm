@echo off

set newJQ=nul

SET count=1

FOR /F "tokens=* USEBACKQ" %%F IN (`git diff packages.config`) DO (
	echo "%%F"|findstr /r /c:"+.*jQuery.*[0-9]*\.[0-9]*\.[0-9]*.*" >nul && set newJQ=%%F
)

if "%newJQ%"=="nul" exit
set newJQ=%newJQ:~33,5%

copy Content\fonts\* ..\DFM.MVC\Assets\Libs\fonts\

copy Scripts\bootstrap.js               ..\DFM.MVC\Assets\Libs\Scripts\
copy Scripts\bootstrap.min.js           ..\DFM.MVC\Assets\Libs\Scripts\

del ..\DFM.MVC\Assets\Libs\Scripts\jquery.intellisense.js
del ..\DFM.MVC\Assets\Libs\Scripts\jquery.js
del ..\DFM.MVC\Assets\Libs\Scripts\jquery.min.js
copy Scripts\jquery-%newJQ%.intellisense.js   ..\DFM.MVC\Assets\Libs\Scripts\jquery.intellisense.js
copy Scripts\jquery-%newJQ%.js                ..\DFM.MVC\Assets\Libs\Scripts\jquery.js             
copy Scripts\jquery-%newJQ%.min.js            ..\DFM.MVC\Assets\Libs\Scripts\jquery.min.js         

copy Scripts\moment-with-locales.js     ..\DFM.MVC\Assets\Libs\Datepicker
copy Scripts\moment-with-locales.min.js ..\DFM.MVC\Assets\Libs\Datepicker
copy Scripts\moment.js                  ..\DFM.MVC\Assets\Libs\Datepicker
copy Scripts\moment.min.js              ..\DFM.MVC\Assets\Libs\Datepicker
