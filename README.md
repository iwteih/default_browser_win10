# default_browser_win10
Set default browser in Windows 10

There is no API or script to set default browser in Windows 10.  
One possible solution is to utilize GPO (Group Policy Object), but it is usually forbidden in company.  
Currently the only way is to open system settting and set the default browser.   
  
This app provides a button in GUI. After clicking the button, the system setttings will show and the app will pick the default browser as you specifed in the config file.  
```xml
    <!--The integer in desiredBrowerIndex should be the index of desired web browser in Default apps -->
    <!--The string in desiredBrowerIndex should be one of the followings-->
    <!--InternetExplorer, Firefox, Chrome, Opera, Safari, Edge-->
    <add key="desiredBrowerIndex" value="3,Chrome"/>
```

![applicationframehost_2017-04-28_11-33-50](https://cloud.githubusercontent.com/assets/5849364/25513283/aa45405a-2c06-11e7-8e04-99187a0b0414.png)
