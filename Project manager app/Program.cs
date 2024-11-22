using Project_manager_app.Classes;
using Project_manager_app.Enums;
using System;
using System.Data.Common;
using System.Threading.Channels;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

var projects = new Dictionary<Project, List<Assignment>>();

var project1 = new Project
{
    Name = "Git projekt",
    Description = "Ovo je Git projekt sa DUMP internshipa.",
    StartDate = DateTime.Now.AddDays(-15),
    EndDate = DateTime.Now.AddDays(-5),
    Status = ProjectStatus.Completed
};

var project2 = new Project
{
    Name = "NET aplikacija",
    Description = "Ovo je NET aplikacija sa DUMP internshipa.",
    StartDate = DateTime.Now.AddDays(-2),
    EndDate = DateTime.Now.AddDays(7),
    Status = ProjectStatus.Active
};

var project3 = new Project
{
    Name = "Projekt manager aplikacija",
    Description = "Ovo je Projekt manager aplikacija sa DUMP internshipa.",
    StartDate = DateTime.Now.AddDays(8),
    EndDate = DateTime.Now.AddDays(16),
    Status = ProjectStatus.Pending
};

var task1 = new Assignment
{
    Name = "Kreirati repozitorij",
    Description = "Kreirati repozitorij na Githubu i klonirati ga na svoje racunalo.",
    Deadline = DateTime.Now.AddDays(-8),
    Status = AssignmentStatus.Completed,
    ExpectedDuration = 5,
    Priority = AssignmentPriority.High
};

var task2 = new Assignment
{
    Name = "Kreirati README",
    Description = "Kreirati README datoteku na Githubu.",
    Deadline = DateTime.Now.AddDays(-5),
    Status = AssignmentStatus.Completed,
    ExpectedDuration = 2,
    Priority = AssignmentPriority.Low
};

var task3 = new Assignment
{
    Name = "Kreirati granu",
    Description = "Kreirati granu koristeci Git.",
    Deadline = DateTime.Now.AddDays(-5),
    Status = AssignmentStatus.Completed,
    ExpectedDuration = 3,
    Priority = AssignmentPriority.Medium
};

var task4 = new Assignment
{
    Name = "Kreirati izbornik korisnici",
    Description = "Kreirati izbornik korisnici, njegov podizbornik i sve funkcionalnosti za: kreiranje, brisanje, uredivanje korisnika itd.",
    Deadline = DateTime.Now.AddDays(7),
    Status = AssignmentStatus.Active,
    ExpectedDuration = 120,
    Priority = AssignmentPriority.High
};

var task5 = new Assignment
{
    Name = "Kreirati izbornik racuni",
    Description = "Kreirati izbornik racuni, njegov podizbornik i sve funkcionalnosti za: kreiranje, brisanje, uredivanje transakcija itd.",
    Deadline = DateTime.Now.AddDays(7),
    Status = AssignmentStatus.Active,
    ExpectedDuration = 150,
    Priority = AssignmentPriority.High
};

var task6 = new Assignment
{
    Name = "Kreirati podizbornik za slanje novca",
    Description = "Kreirati podizbornike za slanje novca interno izmedu racuna i eksterno sa drugim korisnicima.",
    Deadline = DateTime.Now.AddDays(7),
    Status = AssignmentStatus.Active,
    ExpectedDuration = 90,
    Priority = AssignmentPriority.Low
};

var task7 = new Assignment
{
    Name = "Kreirati klasu projekt",
    Description = "Kreirati klasu projekt koja ce stvarati objekt projekt.",
    Deadline = DateTime.Now.AddDays(16),
    Status = AssignmentStatus.Postponed,
    ExpectedDuration = 30,
    Priority = AssignmentPriority.High
};

var task8 = new Assignment
{
    Name = "Kreairati klasu zadatak",
    Description = "Kreirati klasu projekt koja ce stvarati objekt zadatak.",
    Deadline = DateTime.Now.AddDays(16),
    Status = AssignmentStatus.Postponed,
    ExpectedDuration = 30,
    Priority = AssignmentPriority.High
};

var task9 = new Assignment
{
    Name = "Kreirati enume",
    Description = "Kreirati enum za prioritet zadatka, status zadatka i status projekta.",
    Deadline = DateTime.Now.AddDays(16),
    Status = AssignmentStatus.Postponed,
    ExpectedDuration = 10,
    Priority = AssignmentPriority.Medium
};

projects[project1] = new List<Assignment> { task1, task2, task3 };
projects[project2] = new List<Assignment> { task4, task5, task6 };
projects[project3] = new List<Assignment> { task7, task8, task9 };

static void WrongEntry()
{
    Console.WriteLine("Pogresan unos. Pokusaj ponovno.");
    Console.WriteLine();
}

static string ChooseAction()
{
    Console.WriteLine();
    Console.Write("Odaberite radnju: ");

    return Console.ReadLine();
}

static string GoBack()
{
    Console.WriteLine();
    Console.Write("Unesite 0 za povratak: ");

    return Console.ReadLine();
}

static string CheckStringInput(string text)
{
    var value = "";

    while (value == "")
    {
        Console.Write(text);
        value = Console.ReadLine();

        if (value == "")
            Console.Clear();
            WrongEntry();
            
    }

    return value;
}

static DateTime CheckDateInput(string text)
{
    var value = "";
    DateTime result;

    while (!DateTime.TryParse(value, out result))
    {
        Console.Write(text);
        value = Console.ReadLine();

        if (!DateTime.TryParse(value, out _))
        {
            Console.Clear();
            WrongEntry();
        }
    }

    return result;
}

static int CheckIntInput(string text)
{
    var value = "";
    int result;

    while (!int.TryParse(value, out result) || int.Parse(value) < 1)
    {
        Console.Write(text);
        value = Console.ReadLine();

        if (!int.TryParse(value, out _))
        {
            Console.Clear();
            WrongEntry();
        }
        else if (int.Parse(value) < 1)
        {
            Console.Clear();
            Console.WriteLine("Pogresan unos, broj mora biti veci od 0. Pokusajte ponovno.\n");
        }
    }

    return result;
}

static void ProjectsList(Dictionary<Project, List<Assignment>> projects)
{
    foreach (var project in projects)
    {
        Console.WriteLine($"Projekt: {project.Key.Name}");

        if (project.Value.Count > 0)
        {
            Console.WriteLine("Zadatci: ");
        }
        else
        {
            Console.WriteLine("Zadatci: Nema zadataka");
        }

        foreach (var task in project.Value)
        {
            Console.WriteLine($" - {task.Name}");
        }
        Console.WriteLine();
    }
}
static void AllProjects(Dictionary<Project, List<Assignment>> projects)
{
    ProjectsList(projects);

    var decision = GoBack();

    if (decision == "0")
    {
        Console.Clear();
        MainMenu(projects);
        return;
    }
    else
    {
        Console.Clear();
        WrongEntry();
        AllProjects(projects);
        return;
    }
}
static void NewProject(Dictionary<Project, List<Assignment>> projects)
{
    var name = "";
    
    while(name == "" || projects.Keys.Any(project => project.Name.ToLower() == name.ToLower().Trim()))
    {
        name = CheckStringInput("Unesite ime novog projekta: ");
        name = name.Trim();

        if(projects.Keys.Any(project => project.Name.ToLower() == name.ToLower()))
        {
            Console.Clear();
            Console.WriteLine("Pogresan unos. To ime projekta je vec zauzeto, molimo vas unesite drugo ime.\n");
        }
        else if(name == "")
        {
            Console.Clear();
            WrongEntry();
        }
    }

    Console.Clear();

    var description = CheckStringInput("Unesite opis novog projekta: ");

    Console.Clear();

    var startDate = CheckDateInput("Unesite datum pocetka projekta (yyyy.MM.dd HH:mm:ss): ");

    Console.Clear();

    var endDate = CheckDateInput("Unesite datum zavrsetka projekta (yyyy.MM.dd HH:mm:ss): ");

    while(startDate > endDate)
    {
        Console.Clear();
        Console.WriteLine("Pogresan unos. Datum zavrsetka ne moze biti prije nego datum pocetka. Pokusaj ponovno.");
        Console.WriteLine();
        endDate = CheckDateInput("Unesite datum zavrsetka projekta (yyyy.MM.dd HH:mm:ss): ");
    }

    Console.Clear();

    var choice = "";
    ProjectStatus status = ProjectStatus.Active;

    while(choice != "1" && choice != "2" && choice != "3")
    {
        Console.WriteLine("1 - Aktivan\n2 - Na cekanju\n3 - Zavrsen\n");
        Console.Write("Odaberite status projekta: ");
        choice = Console.ReadLine();
        

        switch (choice)
        {
            case "1":
                status = ProjectStatus.Active;
                break;
            case "2":
                status = ProjectStatus.Pending;
                break;
            case "3":
                status = ProjectStatus.Completed;
                break;
            default:
                Console.Clear();
                WrongEntry();
                break;
        }
    }

    var newProject = new Project
    {
        Name = name,
        Description = description,
        StartDate = startDate,
        EndDate = endDate,
        Status = status
    };

    projects.Add(newProject, new List<Assignment>());

    Console.Clear();
    Console.WriteLine($"Projekt => {name} <= uspjesno kreiran!\n");
    /*var decision = "";
   
    while(decision != "0")
    {
        decision = GoBack();

        if (decision != "0")
            Console.Clear();
            Console.WriteLine("Pogresan unos. Pokusajte ponovno."); 
    }

    Console.Clear();*/
    MainMenu(projects);
    return;
}

static void DeleteProject(Dictionary<Project, List<Assignment>> projects)
{
    var projectName = "";
    var success = false;

    while(success == false)
    {
        ProjectsList(projects);
        Console.Write("Upisite ime projekta koji zelite obrisati: ");
        projectName = Console.ReadLine();
        projectName.Trim();
        success = projects.Keys.Any(project => project.Name.ToLower() == projectName.ToLower()? projects.Remove(project): false);

        if (!success)
        {
            Console.Clear();
            Console.WriteLine("Pogresan unos, ne postoji projekt sa tim imenom. Pokusajte ponovno.\n");
        }
        else
        {
            success = true;
            Console.Clear();
            Console.WriteLine($"Projekt => {projectName} <= uspjesno obrisan!\n");
        }
        
    }

    MainMenu(projects);
    return;
}

static void SevenDayDeadlineTasks(Dictionary<Project, List<Assignment>> projects)
{   
    Console.WriteLine("Zadatci sa rokom u sljedecih 7 dana:\n");
    var assignmentsList = projects.SelectMany(project => project.Value).Where(task => task.Deadline > DateTime.Now && task.Deadline < DateTime.Now.AddDays(7)).ToList();

    if (assignmentsList.Count > 0)
    {
        foreach (var assignment in assignmentsList)
        {
            Console.WriteLine($"- {assignment.Name} ({assignment.Deadline})");
        }
    }
    else
    {
        Console.WriteLine("- Nema zadataka");
    }

    var decision = GoBack();

    if (decision != "0")
    {
        Console.Clear();
        WrongEntry();
        SevenDayDeadlineTasks(projects);
        return;
    }

    Console.Clear();
    MainMenu(projects);
    return;
}

static void ProjectsByStatus(Dictionary<Project, List<Assignment>> projects)
{  
    var choice = "";
    var status = ProjectStatus.Active;
    var statusPrint = "";

    while(choice != "1" && choice != "2" && choice != "3")
    {
        Console.WriteLine("1 - Aktivan\n2 - Na cekanju\n3 - Zavrsen\n");
        Console.Write("Odaberite status projekata koje zelite vidjeti: ");
        choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                statusPrint = "aktivni";
                status = ProjectStatus.Active;
                break;
            case "2":
                statusPrint = "na cekanju";
                status = ProjectStatus.Pending;
                break;
            case "3":
                statusPrint = "zavrseni";
                status = ProjectStatus.Completed;
                break;
            default:
                Console.Clear();
                WrongEntry();
                break;
        }
    }

    var decision = "";
    Console.Clear();

    while(decision != "0")
    {
        
        Console.WriteLine($"Svi projekti koji su {statusPrint}:\n");

        var filteredProjects = projects.Keys.Where(project => project.Status == status).ToList();

        if (filteredProjects.Count > 0)
        {
            foreach (var project in filteredProjects)
            {
                Console.WriteLine($"- {project.Name}");
            }
        }
        else
        {
            Console.WriteLine("- Nema projekata");
        }
        
        decision = GoBack();

        if (decision != "0")
        {
            Console.Clear();
            WrongEntry();
        }   
    }

    Console.Clear();
    MainMenu(projects);
    return;
}

static void ChooseProject(Dictionary<Project, List<Assignment>> projects)
{
    if (projects.Count > 0)
    {
        Console.WriteLine("Projekti:\n");

        foreach (var project in projects)
        {
            Console.WriteLine($"- {project.Key.Name}");
        }

        Console.Write("\n\nUnesite ime projekta kojeg zelite odabrati: ");
        var projectName = Console.ReadLine().Trim();

        var filteredProject = projects.Where(project => project.Key.Name.ToLower() == projectName.ToLower()).ToDictionary(project => project.Key, project => project.Value);

        if(filteredProject.Count > 0)
        {
            Console.Clear();
            ProjectMenu(projects, filteredProject);
            return;
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Pogresan unos, ne postoji projekt sa tim imenom. Pokusajte ponovno.\n");
            ChooseProject(projects);
            return;
        }
    }
    else
    {
        Console.WriteLine("Nema projekata za odabrati\n");
        MainMenu(projects);
        return;
    }
    
}

static void ProjectMenu(Dictionary<Project, List<Assignment>> projects, Dictionary<Project, List<Assignment>> choosenProject)
{
    Console.WriteLine($"Odabrani projekt => {choosenProject.Keys.First().Name}\n");

    Console.WriteLine("1 - Prikazi sve zadatke unutar projekta");
    Console.WriteLine("2 - Prikazi detalje projekta");
    Console.WriteLine("3 - Uredi status projekta");
    Console.WriteLine("4 - Dodaj zadatak unutar projekta");
    Console.WriteLine("5 - Obrisi zadatak unutar projekta");
    Console.WriteLine("6 - Prikazi ocekivano trajanje svih aktivnih zadataka unutar projekta");
    Console.WriteLine("0 - Natrag");

    var decision = ChooseAction();

    switch (decision)
    {
        case "1":
            Console.Clear();
            AllProjectTasks(projects, choosenProject);
            return;
        case "2":
            Console.Clear();
            ProjectDetails(projects, choosenProject);
            return;
        case "3":
            Console.Clear();
            UpdateProjectStatus(projects, choosenProject);
            return;
        case "4":
            Console.Clear();
            AddProjectTask(projects, choosenProject);
            return;
        case "5":
            Console.Clear();
            DeleteProjectTask(projects, choosenProject);
            return;
        case "6":
            Console.Clear();
            ActiveProjectTasks(projects, choosenProject);
            return;
        case "0":
            Console.Clear();
            MainMenu(projects);
            return;
        default:
            Console.Clear();
            WrongEntry();
            ProjectMenu(projects, choosenProject);
            return;
    }
}

static void AllProjectTasks(Dictionary<Project, List<Assignment>> projects, Dictionary<Project, List<Assignment>> choosenProject)
{
    Console.WriteLine($"{choosenProject.Keys.First().Name} => Zadatci\n");

    foreach (var task in choosenProject.Values.First())
    {
        Console.WriteLine($"- {task.Name}");
    }

    var decision = GoBack();
    Console.Clear();

    if(decision != "0")
    {
        WrongEntry();
        AllProjectTasks(projects, choosenProject);
        return;
    }

    ProjectMenu(projects, choosenProject);
    return;
}

static void ProjectDetails(Dictionary<Project, List<Assignment>> projects, Dictionary<Project, List<Assignment>> choosenProject)
{
    var projectStatus = "";

    if(choosenProject.Keys.First().Status == ProjectStatus.Active)
    {
        projectStatus = "Aktivan";
    }
    else if(choosenProject.Keys.First().Status == ProjectStatus.Pending)
    {
        projectStatus = "Na cekanju";
    }
    else
    {
        projectStatus = "Zavrsen";
    }

    Console.WriteLine($"{choosenProject.Keys.First().Name} => Detalji\n");

    Console.WriteLine($"Naziv: {choosenProject.Keys.First().Name}\nOpis: {choosenProject.Keys.First().Description}");
    Console.WriteLine($"Datum pocetka: {choosenProject.Keys.First().StartDate}\nDatum zavrsetka: {choosenProject.Keys.First().EndDate}");
    Console.WriteLine($"Status: {projectStatus}\nBroj zadataka: {choosenProject.Values.First().Count}");

    var decision = GoBack();
    Console.Clear();

    if (decision != "0")
    {
        WrongEntry();
        ProjectDetails(projects, choosenProject);
        return;
    }

    ProjectMenu(projects, choosenProject);
    return;
}

static void UpdateProjectStatus(Dictionary<Project, List<Assignment>> projects, Dictionary<Project, List<Assignment>> choosenProject)
{
    if (choosenProject.Keys.First().Status == ProjectStatus.Completed)
    {
        Console.WriteLine("Ne mozete mijenjati status projekta ciji je trenutni status 'Zavrsen'.\n");
        ProjectMenu(projects, choosenProject);
        return;
    }

    var choice = "";
    ProjectStatus status = ProjectStatus.Active;

    while (choice != "1" && choice != "2" && choice != "3")
    {
        Console.WriteLine($"{choosenProject.Keys.First().Name} => Uredi status projekta\n");
        Console.WriteLine("1 - Aktivan\n2 - Na cekanju\n3 - Zavrsen\n");
        Console.Write("Odaberite novi status projekta ili unesite 0, ako zelite odustati: ");
        choice = Console.ReadLine();


        switch (choice)
        {
            case "1":
                status = ProjectStatus.Active;
                break;
            case "2":
                status = ProjectStatus.Pending;
                break;
            case "3":
                status = ProjectStatus.Completed;
                break;
            case "0":
                Console.Clear();
                ProjectMenu(projects, choosenProject);
                return;
            default:
                Console.Clear();
                WrongEntry();
                break;
        }
    }

    var projectUpdate = projects.Keys.First(project => project.Name == choosenProject.Keys.First().Name);
    projectUpdate.Status = status;

    if (status == ProjectStatus.Completed)
    {
        foreach (var task in choosenProject.Values.First())
        {
            task.Status = AssignmentStatus.Completed;
        }
    }

    Console.Clear();
    Console.WriteLine("Status projekta uspjesno ureden!\n");
    ProjectMenu(projects, choosenProject);
    return;
}

static void AddProjectTask(Dictionary<Project, List<Assignment>> projects, Dictionary<Project, List<Assignment>> choosenProject)
{
    var name = "";

    if (choosenProject.Keys.First().Status != ProjectStatus.Completed)
    {
        while (name == "" || choosenProject.Values.First().Any(task => task.Name.ToLower() == name.ToLower().Trim()))
        {
            Console.Write("Unesite ime novog zadatka: ");
            name = Console.ReadLine().Trim();

            if (choosenProject.Values.First().Any(task => task.Name.ToLower() == name.ToLower()))
            {
                Console.Clear();
                Console.WriteLine("Pogresan unos. To ime zadatka je vec zauzeto, molimo vas unesite drugo ime.\n");
            }
            else if (name == "")
            {
                Console.Clear();
                WrongEntry();
            }
        }
    }
    else
    {
        Console.Clear();
        Console.WriteLine("Ne mozete dodavati zadatke unutar projekta ciji je status 'Zavrsen'.\n");
        ProjectMenu(projects, choosenProject);
        return;
    }

    Console.Clear();
    var description = CheckStringInput("Unesite opis novog zadatka: ");

    Console.Clear();
    var deadlineDate = CheckDateInput("Unesite rok izvrsenja novog zadatka (yyyy.MM.dd HH:mm:ss): ");

    while(deadlineDate < choosenProject.Keys.First().StartDate || deadlineDate > choosenProject.Keys.First().EndDate)
    {
        if(deadlineDate < choosenProject.Keys.First().StartDate || deadlineDate > choosenProject.Keys.First().EndDate)
        {
            Console.Clear();
            Console.WriteLine($"Pogresan unos. Rok zadatka ne moze biti manji od pocetnog datuma projekta ({choosenProject.Keys.First().StartDate}) ");
            Console.WriteLine($"ili veci od zavrsnog datuma projekta ({choosenProject.Keys.First().EndDate}). Pokusajte ponovno.\n");
            deadlineDate = CheckDateInput("Unesite rok izvrsenja novog zadatka (yyyy.MM.dd HH:mm:ss): ");
        }
    } 

    Console.Clear();

    var choice = "";
    AssignmentStatus status = AssignmentStatus.Active;

    while (choice != "1" && choice != "2" && choice != "3")
    {
        Console.WriteLine("1 - Aktivan\n2 - Zavrsen\n3 - Odgoden\n");
        Console.Write("Odaberite status novog zadatka ili unesite 0, ako zelite odustati: ");
        choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                status = AssignmentStatus.Active;
                break;
            case "2":
                status = AssignmentStatus.Completed;
                break;
            case "3":
                status = AssignmentStatus.Postponed;
                break;
            case "0":
                Console.Clear();
                ProjectMenu(projects, choosenProject);
                return;
            default:
                Console.Clear();
                WrongEntry();
                break;
        }
    }

    Console.Clear();
    var expectedDuration = CheckIntInput("Unesite ocekivano vrijeme trajanja (u minutama) novog zadatka: ");
    var newTask = new Assignment
    {
        Name = name,
        Description = description,
        Deadline = deadlineDate,
        Status = status,
        ExpectedDuration = expectedDuration
    };

    projects[choosenProject.Keys.First()].Add(newTask);
    Console.Clear();
    Console.WriteLine("Zadatak uspjesno dodan!\n");
    ProjectMenu(projects, choosenProject);
    return;
}

static void DeleteProjectTask(Dictionary<Project, List<Assignment>> projects, Dictionary<Project, List<Assignment>> choosenProject)
{
    var name = "";

    while(!choosenProject.Values.First().Any(task => task.Name.ToLower() == name.Trim().ToLower()))
    {
        Console.WriteLine($"{choosenProject.Keys.First().Name} => Brisanje zadatka\n\nImena zadataka:");
        foreach (var task in choosenProject.Values.First())
        {
            Console.WriteLine($"- {task.Name}");
        }

        Console.Write("\nUnesite ime zadatka kojeg zelite obrisati: ");
        name = Console.ReadLine().Trim();

        if(name == "")
        {
            Console.Clear();
            WrongEntry();
        }
        else if(!choosenProject.Values.First().Any(task => task.Name.ToLower() == name.ToLower()))
        {
            Console.Clear();
            Console.WriteLine("Pogresan unos, ne postoji zadatak sa tim imenom. Pokusajte ponovno.\n");
        }
    }

    var tasks = choosenProject.First().Value;
    var deleteTask = tasks.First(task => task.Name.ToLower() == name.ToLower());
    var decision = "";
    Console.Clear();

    while (decision != "1" && decision != "2")
    {   
        Console.WriteLine($"Jeste li sigurni da zelite obrisati zadatak => {deleteTask.Name}\n");
        Console.WriteLine("1 - DA\n2 - NE\n");
        Console.Write("Odaberite radnju: ");
        decision = Console.ReadLine();

        if(decision != "1" && decision != "2")
        {
            Console.Clear();
            WrongEntry();
        }
    }

    Console.Clear();

    if(decision == "1")
    {
        tasks.Remove(deleteTask);
        
        Console.WriteLine("Zadatak uspjesno obrisan!\n");
    }
    
    ProjectMenu(projects, choosenProject);
    return;
}

static void ActiveProjectTasks(Dictionary<Project, List<Assignment>> projects, Dictionary<Project, List<Assignment>> choosenProject)
{
    Console.WriteLine($"{choosenProject.Keys.First().Name} => Prikaz aktivnih zadataka sa ocekivanim vremenom izvrsenja (u minutama)\n");

    if(choosenProject.Values.First().Any(task => task.Status == AssignmentStatus.Active))
    {
        foreach(var task in choosenProject.First().Value)
        {
            if(task.Status == AssignmentStatus.Active)
            {
                Console.WriteLine($"- {task.Name} => {task.ExpectedDuration}");
            }
        }
    }
    else
    {
        Console.WriteLine("- Nema aktivnih zadataka");
    }

    var decision = GoBack();

    while(decision != "0")
    {
        Console.Clear();
        WrongEntry();
        ActiveProjectTasks(projects, choosenProject);
        decision = GoBack();
    }

    Console.Clear();
    ProjectMenu(projects, choosenProject);
    return;
}

static void ChooseTask(Dictionary<Project, List<Assignment>> projects)
{
    var name = "";

    while(!projects.Values.SelectMany(tasks => tasks).Any(task => task.Name.ToLower() == name.ToLower()))
    {   
        ProjectsList(projects);
        Console.Write("Unesite ime zadatka kojeg zelite odabrati: ");
        name = Console.ReadLine().Trim();
        Console.Clear();

        if(name == "")
        { 
            Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n"); 
        }
        else if(!projects.Values.SelectMany(tasks => tasks).Any(task => task.Name.ToLower() == name.ToLower()))
        {
            Console.WriteLine("Pogresan unos, ne postoji zadatak sa tim imenom. Pokusajte ponovno.\n");
        }
    }

    var choosenTask = projects.Values.SelectMany(tasks => tasks).First(task => task.Name.ToLower() == name.ToLower());

    Console.Clear();
    TaskMenu(projects, choosenTask);
    return;
}

static void TaskMenu(Dictionary<Project, List<Assignment>> projects, Assignment choosenTask)
{
    Console.WriteLine($"Odabrani zadatak => {choosenTask.Name}\n\n1 - Prikazi detalje zadatka\n2 - Uredivanje statusa zadatka\n0 - Natrag");

    var decision = ChooseAction();

    switch (decision)
    {
        case "1":
            Console.Clear();
            TaskDetails(projects, choosenTask);
            return;
        case "2":
            Console.Clear();
            UpdateTaskStatus(projects, choosenTask);
            return;
        case "0":
            Console.Clear();
            MainMenu(projects);
            return;
        default:
            Console.Clear();
            WrongEntry();
            TaskMenu(projects, choosenTask);
            return;
    }
}

static void TaskDetails(Dictionary<Project, List<Assignment>> projects, Assignment choosenTask)
{
    var taskStatus = "";

    if (choosenTask.Status == AssignmentStatus.Active)
    {
        taskStatus = "Aktivan";
    }
    else if (choosenTask.Status == AssignmentStatus.Completed)
    {
        taskStatus = "Zavrsen";
    }
    else
    {
        taskStatus = "Odgoden";
    }

    Console.WriteLine($"{choosenTask.Name} => Detalji\n");

    Console.WriteLine($"Naziv: {choosenTask.Name}\nOpis: {choosenTask.Description}");
    Console.WriteLine($"Rok izvrsavanja: {choosenTask.Deadline}\nOcekivano vrijeme trajanja (u minutama): {choosenTask.ExpectedDuration}");
    Console.WriteLine($"Status: {taskStatus}");

    var decision = GoBack();
    Console.Clear();

    if (decision != "0")
    {
        WrongEntry();
        TaskDetails(projects, choosenTask);
        return;
    }

    TaskMenu(projects, choosenTask);
    return;
}

static void UpdateTaskStatus(Dictionary<Project, List<Assignment>> projects, Assignment choosenTask)
{
    if (choosenTask.Status == AssignmentStatus.Completed)
    {
        Console.WriteLine("Ne mozete mijenjati status zadatka ciji je trenutni status 'Zavrsen'.\n");
        TaskMenu(projects, choosenTask);
        return;
    }

    var choice = "";
    AssignmentStatus status = AssignmentStatus.Active;

    while (choice != "1" && choice != "2" && choice != "3")
    {
        Console.WriteLine($"{choosenTask.Name} => Uredi status zadatka\n");
        Console.WriteLine("1 - Aktivan\n2 - Zavrsen\n3 - Odgoden\n");
        Console.Write("Odaberite novi status zadatka ili unesite 0, ako zelite odustati: ");
        choice = Console.ReadLine();


        switch (choice)
        {
            case "1":
                status = AssignmentStatus.Active;
                break;
            case "2":
                status = AssignmentStatus.Completed;
                break;
            case "3":
                status = AssignmentStatus.Postponed;
                break;
            case "0":
                Console.Clear();
                TaskMenu(projects, choosenTask);
                return;
            default:
                Console.Clear();
                WrongEntry();
                break;
        }
    }
    
    choosenTask.Status = status;

    if (status == AssignmentStatus.Completed)
    {
        var project = projects.First(project => project.Value.Any(task => task.Name == choosenTask.Name));
        if(project.Value.All(task => task.Status == AssignmentStatus.Completed))
        {
            project.Key.Status = ProjectStatus.Completed;
        }
    }  

    Console.Clear();
    Console.WriteLine("Status projekta uspjesno ureden!\n");
    TaskMenu(projects, choosenTask);
    return;
}
static void MainMenu(Dictionary<Project, List<Assignment>> projects)
{
    Console.WriteLine("1 - Prikazi sve projekte");
    Console.WriteLine("2 - Dodaj novi projekt");
    Console.WriteLine("3 - Obrisi projekt");
    Console.WriteLine("4 - Prikazi sve zadatake sa rokom u sljedecih 7 dana");
    Console.WriteLine("5 - Prikazi projekte prema statusu");
    Console.WriteLine("6 - Upravljanje odredenim projektom");
    Console.WriteLine("7 - Upravljanje odredenim zadatkom");
    Console.WriteLine("0 - Izlaz iz aplikacije");

    var decision = ChooseAction();

    switch (decision)
    {
        case "1":
            Console.Clear();
            AllProjects(projects);
            return;
        case "2":
            Console.Clear();
            NewProject(projects);
            return;
        case "3":
            Console.Clear();
            DeleteProject(projects);
            return;
        case "4":
            Console.Clear();
            SevenDayDeadlineTasks(projects);
            return;
        case "5":
            Console.Clear();
            ProjectsByStatus(projects);
            return;
        case "6":
            Console.Clear();
            ChooseProject(projects);
            return;
        case "7":
            Console.Clear();
            ChooseTask(projects);
            return;
        case "0":
            return;
        default:
            Console.Clear();
            WrongEntry();
            MainMenu(projects);
            return;
    }
}

MainMenu(projects);

