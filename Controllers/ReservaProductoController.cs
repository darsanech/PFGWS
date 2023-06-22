using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using PFGWS.Models;
using SQLite;
using System.Security.Claims;

namespace PFGWS.Controllers
{
    public class ReservaProductoController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataFFF.db");
        SyncController syncController = new SyncController();
        ProductoController proController = new ProductoController();

        
        public async Task Post(List<ReservaProducto> nReservaProd, int idestado)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            await db.InsertAllAsync(nReservaProd);
            if(idestado==2 || idestado==6) 
            {
                foreach (ReservaProducto rp in nReservaProd)
                {
                    await proController.BasicPut(rp.producteid, rp.quantitat, false, true);
                }
            }
            await db.CloseAsync();
            await syncController.LoadData();
        }

        bool Comparador(List<ReservaProducto> nReservaProd, List<ReservaProducto> oldReservaProd)
        {

            return true;
        }

        public async Task Put(List<ReservaProducto> nReservaProd, List<ReservaProducto> oldReservaProd, int newidestado, int oldidestado)
        {
            var db = new SQLiteAsyncConnection(databasePath);

            if (nReservaProd.SequenceEqual(oldReservaProd))
            {
                if (newidestado != oldidestado)
                {
                    if((oldidestado == 1 || oldidestado == 7) && (newidestado==2 || newidestado == 6))
                    {
                        foreach (ReservaProducto rp in nReservaProd)
                        {
                            await proController.BasicPut(rp.producteid, rp.quantitat, false, true);
                        }
                    }
                    else if(oldidestado==3 && newidestado == 4)
                    {
                        foreach (ReservaProducto rp in nReservaProd)
                        {
                            await proController.BasicPut(rp.producteid, rp.quantitat, false, false);
                        }
                    }
                }
            }
            else
            {
                Dictionary<int, int> newdic = nReservaProd.ToDictionary(keySelector: x=>x.producteid,elementSelector: x=>x.quantitat);
                Dictionary<int, int> olddic = oldReservaProd.ToDictionary(keySelector: x => x.producteid, elementSelector: x => x.quantitat);
                Dictionary<int, int> result = (from e in newdic.Concat(olddic)
                                               group e by e.Key into g
                                               select new { Name = g.Key, Count = g.Sum(kvp => -kvp.Value) }).ToDictionary(item => item.Name, item => item.Count);
                if(newidestado != oldidestado)
                {
                    if((oldidestado ==5 || oldidestado == 6) && newidestado == 2)
                    {
                        foreach (var res in result)
                        {
                            await proController.BasicPut(res.Key, res.Value, false, false);
                        }
                    }
                }
                else
                {
                    if(newidestado==2 || newidestado == 6 || newidestado == 5)
                    {
                        foreach (var res in result)
                        {
                            await proController.BasicPut(res.Key, res.Value, false, false);
                        }
                    }
                }
                var query = await db.QueryAsync<ReservaProducto>("Delete from ReservaProducto where idreserva="+ nReservaProd.First().idreserva.ToString());
                await db.InsertAllAsync(nReservaProd);
            }
            await db.CloseAsync();
            await syncController.LoadData();
        }
    }
}