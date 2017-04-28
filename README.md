# default_browser_win10
Set default browser in Windows 10

There is no API or script to set default browser in Windows 10.  
One possible solution is to utilize GPO (Group Policy Object), but it is usually forbidden in company.  
Currently the only way is to open system settting and set the default browser.   
  
This app provides a button in GUI. After clicking the button, the system setttings will show and the app will pick the default browser as you specifed in the config file.  
```xml
    <add key="desiredBrowerIndex" value="3"/> <!--chrome in my machine-->
```
