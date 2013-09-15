
SET SPARKLEPACKAGE_DEST=C:\dev\TFS\crm\sparklexrm\sparklexrmcore\Lib\
SET SPARKLEJS_DEST=C:\dev\TFS\crm\sparklexrm\sparklexrmcore\Scripts\
SET SPARKLESTYLES_DEST=C:\dev\TFS\crm\sparklexrm\sparklexrmcore\Styles\
SET SPARKLEIMAGES_DEST=C:\dev\TFS\crm\sparklexrm\sparklexrmcore\Images\
SET SPARKLEHTML_DEST=C:\dev\TFS\crm\sparklexrm\sparklexrmcore\HTML\

SET SPARKLEXRMUI_SOURCE=C:\dev\GitHub\SparkleXrm\SparkleXrmSource\SparkleXrmUI\bin\Debug\
SET SPARKLEXRM_SOURCE=C:\dev\GitHub\SparkleXrm\SparkleXrmSource\SparkleXrm\bin\Debug\
SET SLICK_SOURCE=C:\dev\GitHub\SparkleXrm\SparkleXrmSource\Slick\bin\Debug\

SET SPARKLESCRIPT_SOURCE=C:\dev\GitHub\SparkleXrm\SparkleXrmSource\DebugWeb\WebResources\sparkle_\js\
SET SPARKLEHTML_SOURCE=C:\dev\GitHub\SparkleXrm\SparkleXrmSource\DebugWeb\WebResources\sparkle_\html\
SET SPARKLECSS_SOURCE=C:\dev\GitHub\SparkleXrm\SparkleXrmSource\DebugWeb\WebResources\sparkle_\css\

copy "%SPARKLEXRM_SOURCE%SparkleXrm.dll" "%SPARKLEPACKAGE_DEST%SparkleXrm.dll" /Y

copy "%SPARKLEXRMUI_SOURCE%SparkleXrmUI.dll" "%SPARKLEPACKAGE_DEST%SparkleXrmUI.dll" /Y

copy "%SLICK_SOURCE%Slick.dll" "%SPARKLEPACKAGE_DEST%Slick.dll" /Y

copy "%SPARKLESCRIPT_SOURCE%SparkleXrm.js" "%SPARKLEJS_DEST%SparkleXrm.js" /Y

copy "%SPARKLESCRIPT_SOURCE%SparkleXrmUI.js" "%SPARKLEJS_DEST%SparkleXrmUI.js" /Y

copy "%SPARKLESCRIPT_SOURCE%SparkleXrmUI_Dependancies.js" "%SPARKLEJS_DEST%SparkleXrmUI_Dependancies.js" /Y

