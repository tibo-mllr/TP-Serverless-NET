# TP .NET et Serverless

## Pré-requis

### Codespace (recommandé)
Afin de simplifier le démarrage un [GitHub Codespace](https://github.com/features/codespaces) a été préconfiguré et nécessite un compte GitHub. Notons que les étudiants bénéficient de certains avantages, dont des heures Codespace supplémenaires, via le programme [GitHub Student Developer Pack](https://education.github.com/pack).

1. Effectuez un [fork](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/working-with-forks/fork-a-repo) privé de ce repository, fork sur lequel vous travaillerez pour toute la durée du TP.
   
   ![image](https://github.com/user-attachments/assets/cb2263fe-19f9-4005-98fe-fcad811a0d5e)

3. Assurez-vous d'être dans votre fork en vérifiant l'URL du navigateur.

4. Créez votre Codespace en cliquant sur *Create a codespace on main*. Choisissez la plus petite machine possible: le TP ne demande pas beaucoup de ressource car les tâches de calcul sont surtout effectuées par les bases de données auquelles vous vous connecterez. Si vous utilisez Firefox, désactivez l'*Enhanced Tracking Protection* sur l'URL de votre Codespace sans quoi vous ne pourrez vous y connecter.

![image](https://github.com/user-attachments/assets/337199dd-4223-4326-9e7d-4180c0792082)


### Installation en local
Installez les éléments suivants:

- [.NET Core 8.0](https://dotnet.microsoft.com/download/dotnet-core) (vérifiez que la version de .NET est supportée par Azure Functions)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Azure Functions extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)

### 1. Hello World!
1. Suivez les [instructions officielles](https://docs.microsoft.com/en-us/dotnet/core/get-started) pour créer votre premier programme en .NET. ⚠️ Ajoutez le paramètre `--use-program-main` avec le `dotnet new` pour créer un programme initial avec une structure qui vous facilitera le reste du TP.
3. Naviguez vers le dossier *sample1* et modifiez le programme en y ajoutant une classe *Personne* avec (Testez le snippet *prop* puis la touche *TAB* ):
    1. Une propriété **nom** de type *string*
    2. Une propriété **age** de type *int*
    3. (optionnel) Une méthode `Hello(bool isLowercase)` qui renvoit **"hello *name*, you are *age*"** si **isLowercase** vaut *true*, ou la même chaîne mais en majuscule sinon.
    4. Créez une variable de type *Personne* en lui assignant un nom et un âge ([docs](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/instance-constructors))
    5. Affichez à l'écran **hello *name*, you are *age*** (au lieu du *Hello World!* d'origine)

Pour recompiler et exécuter votre code, depuis le dossier du projet:
> dotnet run

Pour recompiler uniquement
> dotnet build

### 2. NuGet packages
Comme tout langage moderne, .NET bénéficie d'une communauté de développeurs partageant des librairies prêtes à l'emploi, en l'occurence [NuGet](https://nuget.org). Dans cette exercice nous allons utiliser la librairie la plus populaire de NuGet: **newtonsoft.json** qui permet de facilement travailler avec des données au format json.

1. Sur la base du programme Hello World, installez le package **newtonsoft.json** en exécutant la commande `dotnet add package Newtonsoft.Json`
2. Modifiez le programme afin de sérialiser dans une chaîne de caractère la variable de type *Personne* créée précédemment en utilisant la méthode [`JsonConvert.SerializeObject`](https://www.newtonsoft.com/json/help/html/SerializingJSON.htm#JsonConvert) (n'oubliez d'inclure le namespace adéquat en ajoutant `using Newtonsoft.Json` en tête de fichier)
3. (optionnel) Formattez la sérialisation précédente en utilisant l'option `Formatting==Indented`
4. Affichez le json à l'écran à la place du texte précédemment présent

### 3. Traitement d'image locale
Nous allons maintenant utiliser le package [ImageSharp](https://github.com/SixLabors/ImageSharp).

1. Toujours dans le même dossier, ajoutez le package **SixLabors.ImageSharp**
2. En vous basant sur [ces exemples](https://docs.sixlabors.com/articles/imagesharp/gettingstarted.html), redimensionnez une image (ou effectuez une autre transformation!) et sauvegardez-là dans le dossier de votre choix. Pour préciser des noms de chemin Windows, je vous conseille la syntaxe **@"c:\mon chemin\monimage.jpeg"** qui permet de garder lisibles les backslashes
3. (optionnel avancé) La gestion du parallélisme et de l'asynchrone est un point fort de .NET. Utilisez **Parallel.ForEach** pour redimensionner plusieurs images en parallèle. Mesurez les performances.

### 4. Portage vers une Azure Function
Nous allons maintenant porter ce petit programme pour pouvoir l'héberger au sein d'une Azure Function. Elle se déclenchera sur un appel [HttpTrigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob-trigger?tabs=csharp) en POST.

1. Créez un nouveau dossier local (votre ordinateur ou Codespace) *ResizeFunction* 
2. Retournez à la page d'accueil du portail Azure
    - Créez une Function App (ex: *nomprenom-fa*) de type *Consumption* (_pas_ *Flexible Consumption*)
    - Ciblant *.NET 8 (LTS) in-process model*
    - Sur la région *France Central*
    - Système d'exploitation *Linux*
    - Laissez Azure créer un nouveau Storage Account pour héberger le code de la fonction
    - Sélectionnez un hosting *Consumption*
3. Dans l'onglet Azure de Visual Studio Code, section *FUNCTIONS* [créez un nouveau projet Azure Functions depuis Visual Studio Code](https://docs.microsoft.com/fr-fr/azure/azure-functions/create-first-function-vs-code-csharp)
    - Dans le dossier *ResizeFunction*
    - Choisissez *C#*
    - Sélectionnez le runtime *.NET 8.0 (LTS)* - il doit correspondre à celui sélectionné lors de l'étape 2
    - Faites bien attention et sélectionnez `HttpTrigger` (quelques secondes sont nécessaires à l'affichage). C'est ce qui permet d'exposer la fonction en tant qu'API http.    
    - Nommez votre fonction `ResizeHttpTrigger`
    - Choisissez un namespace à votre guise
    - - Sélectionnez *Anonymous* comme type d'authentification
4. Depuis le dossier local *ResizeFunction*, ajoutez le package [ImageSharp](https://github.com/SixLabors/ImageSharp).
5. Modifiez le fichier afin de récupérer le corps (body) de la requête et le charger en tant qu'image dans ImageSharp ([aide](https://stackoverflow.com/questions/54944607/how-to-retrieve-bytes-data-from-request-body-in-azure-function-app))
    - Pour renvoyer les octets de la nouvelle image en tant que réponse à la requête, utilisez **return new FileContentResult(targetImageBytes, "image/jpeg");**
    - Ajoutez les paramètres d'URL **h** et **w** qui permettent à l'appelant de spécifier les dimensions cibles. Ne modifiez pas les noms de ces paramètres: gardez bien **w** et **h** car cela est utilisé pour la notation.
    - Si vous avez des difficultés, collez le contenu du [fichier préparé](https://github.com/lvovan/AA-Serverless-NET/blob/master/ResizeHttpTrigger-incomplete.cs) qui implémente pour vous les éléments très techniques/tuyaux

    - Complétez les différents TODO
    - Récupérez les paramètres **w** et **h** de la requête avec `req.Query[key]` et utilisez les respectivement comme dimensions cibles pour les largeurs et hauteur de la nouvelle image. Attention au typage!
    - Les MIME types sont documentés [ici](https://docs.w3cub.com/http/basics_of_http/mime_types/complete_list_of_mime_types.html)
Récupérez l'adresse de votre fonction depuis le portail Azure en allant sur votre Azure Function, dans la section **Functions**, sélectionnez **ResizeHttpTrigger** et cliquez sur le bouton *Get Function Url* en haut de la page
6. Déployez votre code et appelez fonction avec curl ou via un testeur de web service tel que [Postman](https://www.postman.com/downloads/).

> curl --data-binary "@chaussures_abimees.jpg" -X POST "https://votrefonction.azurewebsites.net/api/ResizeHttpTrigger?w=100&h=100" -v > output.jpeg

7. (optionnel) Changez **AuthorizationLevel.Anonymous** à **AuthorizationLevel.Function** puis récupérez la clé d'API de votre fonction sur le [portail Azure](https://portal.azure.com). Modifiez ensuite votre requête pour qu'elle s'authentifie avec succès.

_Conseil_: Veillez à ce que votre fonction soit propre avec notamment une bonne gestion des erreurs (notamment des paramètres d'entrée invalide) et des retours de [statuts HTTP](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes) adéquats. Votre fonction ne devrait par exemple jamais renvoyer de statuts 500.

### 5. Intégration avec Logic Apps
Le web service étant désormais déployé, voyons comment le réutiliser dans un autre scénario. Nous allons pour cela créer une Logic App (également du serverless) qui surveillera le container d'un Blob Storage Account et déclenchera un appel à notre Azure Function .NET dès qu'un fichier y sera déposé.

1. Depuis le [portail Azure](https://portal.azure.com), crééz une Logic App (Workflow, Consumption/Multi-tenant)
2. Implémentez la Logic App depuis le concepteur graphique (Designer)
    - Vous pouvez utiliser directement le connecteur natif Azure Function, ou le connecteur permettant d'effectuer des appels http (optionnel: essayez les deux). Pour passer les paramètres **w** (width, largeur) et **h** (height, hauteur), utilisez les paramètres *Requêtes* de ces connecteurs
    - Stockez la sortie de la function dans un nouveau blob situé dans un container différent du même Storage Account et nommez le nouveau blob `resized-<nom_du_fichier_source>.jpeg` (que se passerait-il si l'on stockait le nouveau fichier dans le même container que l'ancien?)
    - Utilisez le bouton *Voir le code* pour regarder le fichier décrivant l'application. Notez qu'il s'agit d'un langage déclaratif et non impératif. 
3. Testez votre application en déposant plusieurs fichiers dans le container du Storage Account
4. (optionnel) Ajoutez une condition filtrant les fichiers entrant, en s'assurant que le traitement n'ai lieu que s'ils ont pour extension **.jpg**, **.png**, ou **.bmp**.
5. (optionnel) Le nommage préalablement choisi n'est pas toujours correct car les fichiers créés sont au format jpeg alors que les fichiers sources peuvent être dans un autre format d'image. Pour corriger cela tout en rapprochant alphabétiquement le nom du fichier source et du fichier créé, modifiez le nommage du nouveau fichier ainsi: `[nom_du_fichier_source_sans_extension]-resized.jpeg`

### 6. Rendu - 4 avril 2025
Le TP sera noté et rendu de la manière suivante:
 - Par binôme ou trinôme
 - Pensez à [bien gérer les exceptions](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions)
 - **Bien suivre les instructions** car une partie de l'évaluation est automatisée.
 - Le rendu doit s'effectuer comme décrit ci-dessous.
    - Envoi du rendu par mail:
        - Destinataire: mon adresse ...@centralesupelec.fr
        - Sujet: `TPDOTNET` suivi de vos noms (ex: `TPDOTNET - alice, bob, charles`)
        - CC: Les membres du binôme ou trinôme
        - En pj. l'unique fichier de votre Azure Functions (`ResizeHttpTrigger.cs`)
        - Corps du message:
            1. L'URL https de votre fonction
            2. Le lien vers votre Logic App (celle qui apparaît dans la barre d'adresse du navigateur quand vous êtes sur votre Logic App)
            3. (bonus) les liens vers les vidéos
               
    - Les bonus sont soumis sous forme de vidéo Youtube [unlisted](https://support.google.com/youtube/answer/157177?sjid=4795422317307104878-EU#unlisted&zippy=%2Cunlisted-videos) faisant au plus 3 minutes chacune (le plus court le mieux). Si nécessaire, exécutez le code localement sur votre poste ou augmentez le nombre de CPU de votre Codespace pour mieux mettre en avant l'amélioration des performances.
        - *Bonus 1* : une vidéo démontrant le gain de performance obtenu par la parallélisation sur le traitement local (section 3.3)
        - *Bonus 2* : une vidéo démontrant le fonctionnement de la Logic App lisant des images depuis le Blob Storage (dépôt de plusieurs fichiers, détails du run de la Logic App, apparition des fichiers traités dans le Blob Storage)
    
  _Note:_ Vous pouvez utiliser PowerPoint pour [capturer facilement votre écran](https://www.youtube.com/watch?v=ZCd9fO72vCg). Vous pouvez partager la vidéo sur Teams.
