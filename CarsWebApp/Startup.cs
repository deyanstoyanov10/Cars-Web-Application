namespace CarsWebApp
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.HttpsPolicy;

    using Data;
    using Services;
    using Services.Contracts;
    using System.Threading.Tasks;
    using CarsWebAppML.Model;
    using Microsoft.Extensions.ML;
    using System.Collections.Generic;
    using CarsWebApp.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 15, 0);
            });

            services.AddPredictionEnginePool<ModelInput, ModelOutput>().FromFile("MLModels/MLModel.zip");

            services.AddTransient<ICarService, CarService>();
            services.AddTransient<IListCars, ListCarsService>();
            services.AddTransient<IMessageService, MessageService>();

            services.AddDbContext<CarsDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CarsDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAntiforgery();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            CreateUserRoles(services).Wait();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "api",
                    template: "api/{controller=Values}/{action=Get}");
            });
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //Adding Admin Roles

            await this.CreateRole(roleManager, "Admin");
            await this.CreateRole(roleManager, "Manager");
            await this.CreateRole(roleManager, "Vip");

            //TODO : Assign Admin role to the main User here we have given our newly loregistered login id for Admin management
            var poweruser = new IdentityUser
            {
                UserName = Configuration.GetSection("UserSettings")["UserName"],
                Email = "admin@gmail.com"
            };
            
            string userPassword = Configuration.GetSection("UserSettings")["UserPassword"];
            var _user = await userManager.FindByEmailAsync("admin@gmail.com");
            
            if (_user == null)
            {
                var createPowerUser = await userManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await userManager.AddToRoleAsync(poweruser, "Admin");
                    
                }
            }
        }

        private async Task CreateRole(RoleManager<IdentityRole> roleManager, string role)
        {
            var roleCheck = await roleManager.RoleExistsAsync(role);
            if (!roleCheck)
            {
                //create the roles and seed them to the database   
                await roleManager.CreateAsync(new IdentityRole() { Name = role });
            }

        }

        private async Task AddModels(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService(typeof(CarsDbContext)) as CarsDbContext;
            var models = await context.Models.ToArrayAsync();

            if (models.Length == 0)
            {
                var brandsModels = new List<List<string>>()
                {
                    new List<string> () {"Acura","Integra","Mdx","NSX","Rdx","Rl","Rsx","Slx","Tl","Tsx"},
                    new List<string> (){"Aixam","400","505","600"},
                    new List<string> (){"Alfa Romeo","145","146","147","155","156","159","164","166","33","4C","75","76","8C Competizione","90","Alfetta","Berlina","Brera","Crosswagon q4","Giulia","Giulietta","Gt","Gtv","MiTo","Spider","Sprint","Stelvio","Sud"},
                    new List<string> (){"Aston martin","DBS","Db7","Db9","Rapide","V12 Vantage","V8 Vantage","Vanquish"},
                    new List<string> (){"Audi","100","200","50","60","80","90","A1","A2","A3","A4","A4 Allroad","A5","A6","A6 Allroad","A7","A8","Allroad","Cabriolet","Coupe","E-Tron","Q2","Q3","Q5","Q7","Q8","Quattro","R8","Rs3","Rs4","Rs5","Rs6","Rs7","S2","S3","S4","S5","S6","S7","S8","SQ5","SQ7","SQ8","Tt"},
                    new List<string> (){"Austin","Allegro","Ambassador","Maestro","Maxi","Metro","Mg","Mini","Montego","Princess"},
                    new List<string> (){"Bentley","Arnage","Azure","Bentayga","Continental","Continental gt","Flying Spur","GT Convertible","Mulsanne","T-series"},
                    new List<string> (){"BMW","114","116","118","120","123","125","130","135","1500","1600","1602","1800","2 Active Tourer","2 Gran Tourer","2000","2002","220 d","225","228","230","235","240","2800","315","316","318","320","323","324","325","328","330","335","340","3gt","420","428","430","435","440","5 Gran Turismo","501","518","520","523","524","525","528","530","535","540","545","550","628","630","633","635","640","645","650","700","721","723","725","728","730","732","733","735","740","745","750","760","840","850","Izetta","M","M Coupе","M135","M2","M3","M4","M5","M6","X1","X2","X3","X4","X5","X6","X7","Z1","Z3","Z4","Z8","i3","i8"},
                    new List<string> (){"Bugatti","Chiron","Veyron"},
                    new List<string> (){"Buick","Electra","Invicta","Park avenue","Regal","Rendezvous","Skylark","Skyline"},
                    new List<string> (){"Cadillac","ATS","Allante","BLS","Brougham","CT6","Cts","DTS","Deville","Eldorado","Escalade","Fleetwood","STS","Seville","Srx","XT5","XTS","Xlr"},
                    new List<string> (){"Chevrolet","Alero","Astro","Avalanche","Aveo","Beretta","Blazer","Camaro","Caprice","Captiva","Cavalier","Cobalt","Colorado","Corvette","Cruze","El Camino","Epica","Equinox","Evanda","Gmc","Hhr","Impala","Kalos","Lacetti","Lumina","Malibu","Matiz","Niva","Nova","Nubira","Orlando","Rezzo","Silverado","Spark","Ssr","Suburban","Tacuma","Tahoe","Tracker","Trailblazer","Transsport","Trax","Volt"},
                    new List<string> (){"Chrysler","200","300c","300m","Cherokee","Crossfire","Daytona","Es","Gr.voyager","Grand cherokee","Gts","Interpid","Lebaron","Neon","New yorker","Pacifica","Pt cruiser","Saratoga","Sebring","Stratus","Vision","Voyager","Wrangler"},
                    new List<string> (){"Citroen","2cv","Ax","Axel","Berlingo","Bx","C - Zero","C-Crosser","C-Elysee","C1","C15","C2","C3","C3 Aircross","C3 Picasso","C3 pluriel","C4","C4 AIRCROSS","C4 Cactus","C4 Picasso","C5","C5 Aircross","C6","C8","Cx","DS 3 Crossback","DS 4 Crossback","DS 7 Crossback","DS3","DS4","DS5","DS7","Ds","Evasion","Grand C4 Picasso","Gsa","Gx","Ln","Nemo","Oltcit","Saxo","Visa","Xantia","Xm","Xsara","Xsara picasso","Zx"},
                    new List<string> (){"Corvette","C06 Convertible","C06 Coupe","Powa","Z06"},
                    new List<string> (){"Dacia","1100","1300","1304","1307","1310","1350","Dokker","Duster","Liberta","Lodgy","Logan","Nova","Pickup","Sandero","Solenza"},
                    new List<string> (){"Dodge","Avenger","Caliber","Caravan","Challenger","Charger","Coronet","Dakota","Daytona","Durango","Interpid","Journey","Magnum","Neon","Nitro","Ram","Shadow","Stealth","Stratus","Viper"},
                    new List<string> (){"Ferrari","308","348","360 modena","360 spider","458 Italia","488","599","812 Superfast","California","Enzo","F12berlinetta","F430","F456m","F575m maranello","F612 scaglietti","FF","GTC4Lusso","LaFerrari","Mondial 8","Testarossa"},
                    new List<string> (){"Fiat","1100","124","125","126","127","128","131","132","1400","1500","1800","500","500L","500X","600","650","750","Albea","Argenta","Barchetta","Bertone","Brava","Bravo","Campagnola","Cinquecento","Coupe","Croma","Doblo","Duna","Fiorino","Freemont","Fullback","Idea","Linea","Marea","Multipla","Palio","Panda","Punto","Qubo","Regata","Ritmo","Scudo","Sedici","Seicento","Siena","Stilo","Strada","Tempra","Tipo","Topolino","Ulysse","Uno"},
                    new List<string> (){"Ford","12m","15m","17m","20m","Aerostar","B-Max","Bronco","C-max","Capri","Connect","Consul","Cortina","Cosworth","Cougar","Countur","Courier","Crown victoria","EcoSport","Ecoline","Edge","Escape","Escort","Everest","Excursion","Expedition","Explorer","F150","F250","F350","F450","F550","F650","F750","Fiesta","Flex","Focus","Fusion","Galaxy","Granada","Ka","Kuga","Maverick","Mercury","Mondeo","Mustang","Orion","Probe","Puma","Ranger","Rs","S-Max","Scorpio","Sierra","Sportka","Streetka","Taunus","Taurus","Thunderbird","Windstar","Zephyr"},
                    new List<string> (){"Gmc","Acadia","Canyon","Envoy","Jimmy","Saturn","Savana","Sierra","Sonoma","Terrain","Tracker","Typhoon","Yukon"},
                    new List<string> (){"Honda","Accord","Cbr","Cbx","City","Civic","Civic ballade","Concerto","Cr-v","Crosstour","Crx","Crz","Element","Elysion","Fit","Fr-v","Hr-v","Insight","Integra","Jazz","Legend","Logo","Nsx","Odyssey","Passport","Pilot","Prelude","Quintet","Ridgeline","S2000","Shuttle","Stream"},
                    new List<string> (){"Hummer","H1","H2","H3"},
                    new List<string> (){"Hyundai","Accent","Atos","Coupe","Elantra","Equus","Excel","Galloper","Genesis","Getz","Grace","Grandeur","I10","I20","I30","I40","IX35","IX55","Ioniq","Ix20","Kona","Landskape","Lantra","Matrix","Pony","Porter","S","Santa fe","Santamo","Sonata","Sonica","Stelar","Tb","Terracan","Trajet","Tucson","Veloster ","Veracruz","Xg"},
                    new List<string> (){"Infiniti","Ex30","Ex35","Ex37","Fx 30","Fx 35","Fx 37","Fx 45","Fx 50","Fx45","G coupe","G sedan","Q30","Q45","QX30","QX50","QX56","QX60","QX70","QX80","Qx","Qx4"},
                    new List<string> (){"Isuzu","Amigo","D-max","Gemini","Piazza","Pickup","Rodeo","Tfs","Trooper","Vehi cross"},
                    new List<string> (){"Iveco","Massive"},
                    new List<string> (){"Jaguar","Daimler","Daimler double six","Daimler six","E-pace","F-PACE","F-Type","I-Pace","S-type","Sovereign","Super v8","X-type","XE","Xf","Xj","Xjr","Xjs","Xjsc","Xk8","Xkr"},
                    new List<string> (){"Jeep","Cherokee","Commander","Compass","Grand cherokee","Patriot","Renegade","Wrangler"},
                    new List<string> (){"Kia","Avella delta","Cadenza","Carens","Carnival","Ceed","Cerato","Clarus","Forte","Joecs","Joyce","Magentis","Mohave","Morning","Niro","Opirus","Optima","Picanto","Pride","Pro ceed","Quoris","Ray","Retona","Rio","Sedona","Sephia","Shuma","Sorento","Soul","Spectra","Sportage","Stinger","Stonic","Venga","Visto","X-Trek"},
                    new List<string> (){"Koenigsegg","Agera","CC","Jesko","One:1","Regera" },
                    new List<string> (){"Lada","1200","1300","1500","1600","2101","21011","21012","21013","21015","2102","2103","2104","21043","2105","21051","21053","2106","21061","21063","2107","21074","2108","21083","2109","21093","21099","2110","21213","Granta","Kalina","Niva","Nova","Oka","Priora","Samara","Urban","Vesta"},
                    new List<string> (){"Lamborghini","Aventador","Countach","Diablo","Gallardo","Huracan","Murcielago","Reventon","Urus","Veneno"},
                    new List<string> (){"Lancia","A112","Ardea","Aurelia","Beta","Dedra","Delta","Flaminia","Flavia","Fulvia","Kappa","Lybra","Musa","Phedra","Prisma","Thema","Thesis","Unior","Y","Y10","Ypsilon","Zeta"},
                    new List<string> (){"Land Rover","Defender","Discovery","Freelander","Range Rover Evoque","Range Rover Sport","Range Rover Velar","Range rover"},
                    new List<string> (){"Lexus","CT200h","Es","Gs","Gx470","Is","LC","LFA","LS","Lx","NX","RC","RX330","Rx","Rx300","Rx350","Rx400h","Rx450","Sc","UX"},
                    new List<string> (){"Lincoln","Continental","Ls","MKC","MKS","Mark","Mark Lt","Mark lt","Mkx","Mkz","Navigator","Town car","Zephyr"},
                    new List<string> (){"Maserati","3200 gt","Biturbo","Coupe gt","Ghibli","GranCabrio","GranTurismo","Gransport","Levante","Quattroporte","Spyder","Zagato"},
                    new List<string> (){"Maybach","57","62","650","S 560"},
                    new List<string> (){"Mazda","121","2","3","323","5","6","626","929","B2200","B2500","B2600","BT-50","CX-5","CX-7","CX-9","Demio","Mpv","Mx-3","Mx-5","Mx-6","Premacy","Rx-7","Rx-8","Tribute","Xedos","СХ-3"},
                    new List<string> (){"McLaren","540C Coupe","570S Coupe","650S","675LT","F1","MP4-12C","P1"},
                    new List<string> (){"Mercedes-Benz","110","111","113","114","115","116","123","124","126","126-260","150","170","180","190","200","220","230","240","250","260","280","290","300","320","350","380","420","450","500","560","600","A 140","A 150","A 160","A 170","A 180","A 190","A 200","A 210","A 220","A 250","A45 AMG","AMG GT","AMG GT C","AMG GT R","AMG GT S","Adenauer","B 150","B 160","B 170","B 180","B 200","B 220","B 250","C 160","C 180","C 200","C 220","C 230","C 240","C 250","C 270","C 280","C 30 AMG","C 300","C 32 AMG","C 320","C 350","C 36 AMG","C 400","C 43 AMG","C 450 AMG","C 55 AMG","C 63 AMG","CL 230","CL 320","CL 420","CL 500","CL 55 AMG","CL 600","CL 63 AMG","CL 65 AMG","CLA 180","CLA 200","CLA 220","CLA 250","CLA 45 AMG","CLC 160","CLC 180","CLC 200","CLC 220","CLC 230","CLC 250","CLC 350","CLK 55 AMG","CLK 63 AMG","CLS 250","CLS 300","CLS 320","CLS 350","CLS 400","CLS 450","CLS 500","CLS 53 AMG","CLS 55","CLS 55 AMG","CLS 63","CLS 63 AMG","Citan","E 200","E 220","E 230","E 240","E 250","E 260","E 270","E 280","E 290","E 300","E 320","E 350","E 36 AMG","E 400","E 420","E 43 AMG","E 430","E 450","E 50 AMG","E 500","E 53 AMG","E 55","E 55 AMG","E 60","E 60 AMG","E 63 AMG","EQC","G 230","G 240","G 250","G 270","G 280","G 290","G 300","G 320","G 350","G 36 AMG","G 400","G 500","G 55 AMG","G 63 AMG","G 65 AMG","GL 320","GL 350","GL 420","GL 450","GL 500","GL 55 AMG","GL 63 AMG","GLA 180","GLA 200","GLA 220","GLA 250","GLA 45 AMG","GLC 220","GLC 250","GLC 300","GLC 350","GLC 43 AMG","GLC 63 AMG","GLE 250","GLE 350","GLE 400","GLE 43 AMG","GLE 450 AMG","GLE 63 AMG","GLE 63 S AMG","GLE Coupe","GLS","GLS 350","GLS 400","GLS 500","GLS 63 AMG","ML 230","ML 250","ML 270","ML 280","ML 300","ML 320","ML 350","ML 400","ML 420","ML 430","ML 450","ML 500","ML 55 AMG","ML 63 AMG","R 280","R 300","R 320","R 350","R 500","R 63 AMG","S 250","S 280","S 300","S 320","S 350","S 400","S 420","S 430","S 450","S 500","S 55 AMG","S 550","S 560","S 600","S 63","S 63 AMG","S 65","S 65 AMG","SL 400","SL 500","SL 55 AMG","SL 60 AMG","SL 63 AMG","SL 65 AMG","SLC","SLK","SLK 32 AMG","SLK 55 AMG","SLR","SLS","SLS AMG","Unimog","Vaneo","X-Klasse"},
                    new List<string> (){"Mini","Clubman","Cooper","Cooper cabrio","Cooper s","Cooper s cabrio","Countryman","Coupe","D one","One","One cabrio","Paceman"},
                    new List<string> (){"Mitsubishi","3000 gt","ASX","Carisma","Colt","Cordia","Eclipse","Eclipse Cross","Galant","Grandis","I-MiEV","L200","Lancer","Mirage","Montero","Outlander","Pajero","Pajero pinin","Pajero sport","Sapporo","Sigma","Space gear","Space runner","Space star","Space wagon","Starion","Tredia"},
                    new List<string> (){"Nissan","100 nx","200 sx","240 z","280 z","300 zx","350z","370Z","Almera","Almera tino","Altima","Armada","Bluebird","Cedric","Cherry","Cube","Figaro","Frontier","Gt-r","Juke","Kubistar","Laurel","Leaf ","Maxima","Micra","Murano","NP300","Navara","Note","Pathfinder","Patrol","Pickup","Pixo","Prairie","Primera","Pulsar","Qashqai","Rogue","Sentra","Serena","Silvia","Skyline","Stantza","Sunny","Teana","Terrano","Tiida","Titan crew cab","Titan king","Versa","X-trail","Xterra","e-NV200"},
                    new List<string> (){"Opel","Adam","Admiral","Agila","Ampera","Antara","Ascona","Astra","Calibra","Campo","Cascada","Combo","Commodore","Corsa","Crossland X","Diplomat","Frontera","Grandland X","Gt","Insignia","Kadett","Kapitaen","Karl","Manta","Meriva","Mokka","Monterey","Monza","Omega","Rekord","Senator","Signum","Sintra","Speedster","Tigra","Vectra","Zafira"},
                    new List<string> (){"Pagani","Huayra","Zonda" },
                    new List<string> (){"Peugeot","1007","104","106","107","108","2008","202","204","205","206","207","208","3008","301","304","305","306","307","308","309","4007","4008","402","403","404","405","406","407","5008","504","505","508","604","605","607","806","807","Bipper","Expert","Partner","RCZ","Range","Rifter","Traveler","iOn"},
                    new List<string> (){"Pontiac","Aztec","Bonneville","Fiero","Firebird","G6","Grand am","Grand prix","Gto","Lemans","Solstice","Sunbird","Sunfire","Tempest","Torrent","Trans am","Trans sport","Vibe"},
                    new List<string> (){"Porsche","356 Speedster","911","918 Spyder","924","928","935","944","956","968","991","993","996","Boxster","Carrera","Cayenne","Cayman","Macan","Panamera"},
                    new List<string> (){"Renault","10","11","12","14","16","18","19","20","21","25","29","30","4","5","8","9","Alpine","Avantime","Bakara","Bulgar","Captur","Caravelle","Chamade","Clio","Espace","Express","Floride","Fluence","Fuego","Grand espace","Grand scenic","Kadjar","Kangoo","Koleos","Laguna","Laguna Coupe","Latitude","Megane","Modus","Nevada","Rapid","Safrane","Scenic","Scenic rx4","Symbol","Talisman","Twingo","Twizy","Vel satis","Wind","Zoe"},
                    new List<string> (){"Rolls-Royce","Cullinan","Dawn","Ghost","Phantom","Silver Seraph","Wraith"},
                    new List<string> (){"Saab","9-3","9-4X","9-5","9-7x","900","9000"},
                    new List<string> (){"Seat","Alhambra","Altea","Arona","Arosa","Ateca","Cordoba","Cupra","Exeo","Fura","Ibiza","Inka","Leon","Malaga","Marbella","Mii","Ronda","Tarraco","Terra","Toledo","Vario"},
                    new List<string> (){"Skoda","100","1000","105","120","125","130","135","136","Citigo","Fabia","Favorit","Felicia","Forman","Karoq","Kodiaq","Octavia","Praktik","Rapid","Roomster","Superb","Yeti"},
                    new List<string> (){"Smart","Forfour","Fortwo","Mc","Roadster"},
                    new List<string> (){"SsangYong","Actyon","Actyon Sports", "Chairman", "Korando","Korando Sports","Kyron","Musso","Rexton", "Rodius", "Tivoli","XLV"},
                    new List<string> (){"Subaru","1800","B9 tribeca","BRZ","Baja","E12","Forester","G3x justy","Impreza","Justy","Legacy","Leone","Levorg","Libero","Outback","Svx","Trezia","Vivio","XT","XV"},
                    new List<string> (){"Suzuki","Alto","Baleno","Celerio","Forenza","Grand vitara","Ignis","Jimny","Kizashi","Liana","Maruti","Reno","SX4","SX4 S-Cross","Samurai","Santana","Sg","Sidekick","Sj","Splash","Swift","Vitara","Wagon r","X-90","XL-7"},
                    new List<string> (){"Tata","Aria","Estate","Indica","Mint","Nano","Safari","Sierra","Sumo","Telcoline","Xenon"},
                    new List<string> (){"Tesla","Model 3","Model S","Model X","Roadster","Roadster Sport"},
                    new List<string> (){"Toyota","4runner","Auris","Avalon","Avensis","Avensis verso","Aygo","C-HR","Camry","Carina","Celica","Corolla","Corolla Matrix","Corolla verso","Cressida","Crown","Fj cruiser","GT86","Harrier","Highlander","Hilux","IQ","Land cruiser","Matrix","Mr2","Paseo","Picnic","Previa","Prius","Rav4","Scion","Sequoia","Sienna","Starlet","Supra","Tacoma","Tercel","Tundra","Urban Cruiser","Venza","Verso","Verso S","Yaris","Yaris verso"},
                    new List<string> (){"Volkswagen","1200","1300","1302","1303","1500","1600","Alltrack","Amarok","Arteon","Atlas","Bora","Caddy","Corrado","Derby","Eos","Fox","Golf","Golf Plus","Golf Variant","Jetta","K 70","Karmann-ghia","Lupo","Multivan","New beetle","Passat","Phaeton","Polo","Rabbit","Santana","Scirocco","Sharan","Sportsvan","T-Cross","T-Roc","Taro","Tiguan","Touareg","Touran","Up","Vento"},
                    new List<string> (){"Volvo","142","144","145","164","1800 es","240","244","245","262 c","264","340","343","344","345","360","440","460","480","66","740","744","745","760","765","770","780","850","940","960","C30","C70","P 1800","S40","S60","S70","S80","S90","V40","V40 Cross Country","V50","V60","V60 Cross Country","V70","V90","V90 Cross Country","XC40","XC60","Xc70","Xc90"}
                };

                for (int i = 0; i < brandsModels.Count; i++)
                {
                    context.Brands.Add(new Brand() { BrandType = brandsModels[i][0] });
                    context.SaveChanges();
                }

                for (int i = 1; i <= brandsModels.Count; i++)
                {
                    for (int j = 1; j < brandsModels[i - 1].Count; j++)
                    {
                        context.Models.Add(new Model() { ModelType = brandsModels[i - 1][j], BrandId = i });
                        context.SaveChanges();
                    }
                    context.SaveChanges();
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
