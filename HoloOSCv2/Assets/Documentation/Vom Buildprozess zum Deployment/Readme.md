# Vom Erstellungsprozess bis zum Deployment
### Kurzeinführung zum Entwicklungsprozess für Mixed-Reality-Applikationen mit der Microsoft HoloLens

Hinweis: Es wird davon ausgegangen, dass alle notwendigen Tools bereits installiert sind und die Anweisungen in "Getting started with MRTK" durchgeführt worden sind (https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/GettingStartedWithTheMRTK.html)
![alt test](https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/1_Required%20steps%20to%20import%20MRTK.PNG "Required Steps")

## Unity-Einstellungen

### Aktivierung des Mixed-Reality-Supports
Damit die Anwendung Mixed-Reality unterstützen kann, muss manuell der Virtual-Reality-Support aktiviert über die Player-Settings aktiviert werden (Playersettings: Edit->Project Settings)
![](https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/2_ProjectSettings.PNG)
![]
(https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/3_Adding_XR.PNG "Adding XR")

### Hinzufügen von Fähigkeiten
Wird beispielsweise das Mikrofon, die Webcam oder Internet in der MRTK-Applikation verwendet, müssen vor dem Buildprozess diese ausgewählt werden. Hinweis: Wird erst nachträglich bekannt, dass weitere Fähigkeiten notwendig sind, ist es gegebenenfalls nötig, einen bereits vorhandenen Buildordner zu löschen, da Fähigkeiten nicht automatisch dem aktuellen Buildordner hinzugefügt werden. 
![](https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/3_Adding_Capabilities.PNG "Adding Capabilities")


### Empfehlung der Qualitätseinstellungen
In vielen Foren wurde empfohlen, den Qualitätslevel auf sehr niedrig einzustellen für eine bessere Performance. Qualitätseinbußen konnten wir dadurch nicht feststellen.
![](https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/4_Quality.PNG)

### Änderung des Package-Names
Damit nicht nur eine Applikation auf der HoloLens installiert sein kann, muss der Paketname in Unity angepasst werden. Es reicht nicht aus, nur den Projektnamen im Deploymentprozess zu ändern.
![](https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/3_Package_Name.PNG)

## Buildprozess
Sind alle notwendigen Vorbedingungen erfüllt, kann der Buildprozess starten. Durch diesen Prozess wird die Nutzeranwendung "gebaut".
![](https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/5_1_Buildprozess_starten.PNG)
Es wird empfohlen, einen neuen Ordner im Projekt für den Build anzulegen.
![]
(https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/5_2_Buildprozess_Appordner.PNG)
## Deployment
Um den Build nun auf die HoloLens zu spielen, beginnt der sogenannte Deployment-Prozess. Dazu muss die die Solutiondatei des Buildprozess geöffnet werden (öffnet sich über Visual Studio) und der Deploymentprozess eingestellt werden (in diesem Fall ist die HoloLens per USB-Kabel angeschlossen)
![](https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/6_1_Deployment_Solution_%C3%B6ffnen.PNG "SLN öffnen")
![](https://github.com/Jindorf/HoloOSCV2/blob/master/HoloOSCv2/Assets/Documentation/images/Deploying%20to%20HoloLens/6_2_Debugeinstellungen.PNG)





    
