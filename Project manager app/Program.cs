using Project_manager_app.Classes;
using Project_manager_app.Enums;
using System;
using System.Data.Common;
using System.Threading.Channels;

var projects = new Dictionary<Project, List<Assignment>>();

var project1 = new Project
{
    Name = "Git projekt",
    Description = "Ovo je Git projekt sa DUMP internshipa.",
    StartDate = DateTime.Now.AddDays(-15),
    EndDate = DateTime.Now.AddDays(-5),
    Status = ProjectStatus.Active
};

var project2 = new Project
{
    Name = "NET aplikacija",
    Description = "Ovo je NET aplikacija sa DUMP internshipa.",
    StartDate = DateTime.Now.AddDays(-2),
    EndDate = DateTime.Now.AddDays(7),
    Status = ProjectStatus.Pending
};

var project3 = new Project
{
    Name = "Projekt manager aplikacija",
    Description = "Ovo je Projekt manager aplikacija sa DUMP internshipa.",
    StartDate = DateTime.Now.AddDays(-2),
    EndDate = DateTime.Now.AddDays(12),
    Status = ProjectStatus.Completed
};

var task1 = new Assignment
{
    Name = "Kreairati repozitorij",
    Description = "Kreirati repozitorij na Githubu i klonirati ga na svoje racunalo.",
    Deadline = DateTime.Now.AddDays(-8),
    Status = AssignmentStatus.Completed,
    ExpectedDuration = 7,
    Priority = AssignmentPriority.High
};

var task2 = new Assignment
{
    Name = "Kreairati README",
    Description = "Kreirati README datoteku na Githubu.",
    Deadline = DateTime.Now.AddDays(-8),
    Status = AssignmentStatus.Completed,
    ExpectedDuration = 7,
    Priority = AssignmentPriority.Low
};

var task3 = new Assignment
{
    Name = "Kreairati granu",
    Description = "Kreirati granu koristeci Git.",
    Deadline = DateTime.Now.AddDays(-8),
    Status = AssignmentStatus.Completed,
    ExpectedDuration = 7,
    Priority = AssignmentPriority.Medium
};

var task4 = new Assignment
{
    Name = "Kreairati izbornik korisnici",
    Description = "Kreirati izbornik korisnici, njegov podizbornik i sve funkcionalnosti za: kreiranje, brisanje, uredivanje korisnika itd.",
    Deadline = DateTime.Now.AddDays(7),
    Status = AssignmentStatus.Active,
    ExpectedDuration = 7,
    Priority = AssignmentPriority.High
};

var task5 = new Assignment
{
    Name = "Kreairati izbornik racuni",
    Description = "Kreirati izbornik racuni, njegov podizbornik i sve funkcionalnosti za: kreiranje, brisanje, uredivanje transakcija itd.",
    Deadline = DateTime.Now.AddDays(7),
    Status = AssignmentStatus.Active,
    ExpectedDuration = 7,
    Priority = AssignmentPriority.High
};

var task6 = new Assignment
{
    Name = "Kreirati podizbornik za slanje novca",
    Description = "Kreirati podizbornike za slanje novca interno izmedu racuna i eksterno sa drugim korisnicima.",
    Deadline = DateTime.Now.AddDays(7),
    Status = AssignmentStatus.Completed,
    ExpectedDuration = 7,
    Priority = AssignmentPriority.Low
};

var task7 = new Assignment
{
    Name = "Kreirati klasu projekt",
    Description = "Kreirati klasu projekt koja ce stvarati objekt projekt.",
    Deadline = DateTime.Now.AddDays(16),
    Status = AssignmentStatus.Postponed,
    ExpectedDuration = 7,
    Priority = AssignmentPriority.High
};

var task8 = new Assignment
{
    Name = "Kreairati klasu zadatak",
    Description = "Kreirati klasu projekt koja ce stvarati objekt zadatak.",
    Deadline = DateTime.Now.AddDays(16),
    Status = AssignmentStatus.Postponed,
    ExpectedDuration = 7,
    Priority = AssignmentPriority.High
};

var task9 = new Assignment
{
    Name = "Kreirati enume",
    Description = "Kreirati enum za prioritet zadatka, status zadatka i status projekta.",
    Deadline = DateTime.Now.AddDays(16),
    Status = AssignmentStatus.Postponed,
    ExpectedDuration = 7,
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
            Console.Clear();
            WrongEntry();

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
        Console.WriteLine("Odaberite status projekta:\n\n1 - Aktivan\n2 - Na cekanju\n3 - Zavrsen\n");
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
        Console.WriteLine("Odaberite status projekata koje zelite vidjeti:\n\n1 - Aktivan\n2 - Na cekanju\n3 - Zavrsen\n");
        Console.Write("Odaberite status projekta: ");
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
            Console.WriteLine($"Odabrani projekt => {filteredProject.Keys.First().Name}\n");
            ProjectMenu(filteredProject);
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

static void ProjectMenu(Dictionary<Project, List<Assignment>> projects)
{
    Console.WriteLine(projects.Keys.First().Name);
}
static void MainMenu(Dictionary<Project, List<Assignment>> projects)
{
    Console.WriteLine("1 - Ispisi sve projekte");
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
            // TaskMenu();
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

