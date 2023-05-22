using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using SQLite;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProductoController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataF.db");

        SyncController syncController = new SyncController();

        [HttpGet]
        public async Task<IEnumerable<Producto>> Get()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<Producto>().ToListAsync();
            await db.CloseAsync();

            return query;
        }
        [HttpPut]
        public async Task BasicPut(int producteid, int mod, bool total, bool sale) //false disponible-true total  //sale=true resta, sale=False es recogido
        {
            var db = new SQLiteAsyncConnection(databasePath);
            Producto prod = await db.Table<Producto>().FirstOrDefaultAsync(x => x.producteid == producteid);
            if (total)
            {
                prod.total += mod;
                await db.UpdateAsync(prod);
                await syncController.LoadData();
            }
            else
            {
                if (sale)
                {
                    prod.disponible -= mod;
                }
                else
                {
                    prod.disponible += mod;
                }
                await db.UpdateAsync(prod);

            }
            await db.CloseAsync();

        }
        [HttpPut]
        [Route("/api/Producto/MegaPut")]

        public async Task MegaPut(List<Reserva> r) //false disponible-//-true total
        {

            if (r.Count()==1 && (r[0].estadoid==2 || r[0].estadoid == 6))
            {
                Dictionary<int, int> ProductosPHList = StringtoPPH(r[0].productes,true).Result;
                foreach(var producte in ProductosPHList)
                {
                    await BasicPut(producte.Key, producte.Value, false, true);
                }
            }
            else if(r.Count()>1)
            {
                bool sale = ((r[0].estadoid == 2 || r[0].estadoid == 6) && r[1].estadoid == 1);
                if(r[0].productes!= r[1].productes)
                {
                    Dictionary<int, int>  ProductosPHListnew = StringtoPPH(r[0].productes,false).Result;
                    Dictionary<int, int> ProductosPHListold = StringtoPPH(r[1].productes,true).Result;
                    Dictionary<int, int> result = (from e in ProductosPHListnew.Concat(ProductosPHListold)
                                                   group e by e.Key into g
                                                   select new { Name = g.Key, Count = g.Sum(kvp => kvp.Value) }).ToDictionary(item => item.Name, item => item.Count); ;
                    foreach (var producte in result)
                    {
                        await BasicPut(producte.Key, producte.Value, false, sale);
                    }
                }
                else
                {
                    if (sale)
                    {
                        Dictionary<int, int> ProductosPHListnew = StringtoPPH(r[0].productes, true).Result;
                        foreach (var producte in ProductosPHListnew)
                        {
                            await BasicPut(producte.Key, producte.Value, false, true);
                        }
                    }
                    else if(r[0].estadoid == 4 && (r[1].estadoid == 2 || r[1].estadoid == 3))
                    {
                        Dictionary<int, int> ProductosPHListnew = StringtoPPH(r[0].productes, true).Result;
                        foreach (var producte in ProductosPHListnew)
                        {
                            await BasicPut(producte.Key, producte.Value, false, false);
                        }
                    }
                    
                }

            }
            await syncController.LoadData();

        }
        private async Task<Dictionary<int, int>> StringtoPPH(string productosreserva,bool neww)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<Producto>().ToListAsync();
            Dictionary<int,int> ProductosPHList = new Dictionary<int, int>();
            string[] Productes = productosreserva.Split(", ");
            foreach (string s in Productes)
            {
                string[] Producte = s.Split(" x");
                ProductoPH niu = new ProductoPH();
                niu.productoid = query.FirstOrDefault(x => x.productoname == Producte[0]).producteid;
                if (Producte.Length == 1)
                {
                    niu.cantidad = 1;
                }
                else
                {
                    niu.cantidad = Int32.Parse(Producte[1]);
                }
                if (neww)
                {
                    ProductosPHList.Add(niu.productoid, niu.cantidad);
                }
                else
                {
                    ProductosPHList.Add(niu.productoid, -niu.cantidad);
                }
            }
            return ProductosPHList;
        }
    }
}
