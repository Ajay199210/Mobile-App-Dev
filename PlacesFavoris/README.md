# PlaceFavoris

À partir de cette application, l'utilisateur.trise peut enregistrer des places qui lui sont favoris dans la carte. Une place enregistrée sur la carte peut être :

- Une place que l'utilisateur.trise a déjà visitée
- Une place que l'utilisateur.trise souhaite visiter un jour
- En endroit connu que l'utilisateur.trise souhaite simplement marquer sur la carte mais ne l'a pas visité et ne souhaite pas le faire (au moins pour le moment)

Pour être capable d'utiliser l'application, l'utilisateur.trise a besoin de créer un compte à partir de l'application. Le compte qui sera créé doit être enregistré dans le service Google Firebase Authentication, de même l'authentification de l'utilisateur.trise doit se faire via le service Google Firebase Authentication. Toutes les autres informations de l'application sont enregistrées dans une BD SQLite locale.

***Note :** Svp notez que vous devez remplacez la clé API dans le fichier AndroidManifest.xml par le votre afin d'utiliser l'API de géolocalisation sur un émulateur ou un appreil Android. La même approche doit être utilisée pour change la clé dans le fichier GoogleService-Info.plist afin de tester sur les systèmes iOS*

*AndroidManifest.xml*

`<meta-data android:name="com.google.android.geo.API_KEY" android:value="{Your_API_KEY}" />`

*GoogleService-Info.plist*

`<key>API_KEY</key>`
`<string>{Your_API_Key}</string>`