using MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MVVM
{
    class ServiceModel
    {
        public event EventHandler<FlickCamerasEventArgs> GetCamerasCompleted;
        // método asincrono que devuelve lista de cámaras
        public void getCameras()
        {
            // empezamos  acrear la base de datos,isostore es la ruta de almacenamiento aisaldo
            var db = new CameraDataContext("isostore:/cameras.sdf");

            // verificamos si las base de datos existe
            if (db.DatabaseExists())
            {
                // si es así entonces leemos todo con ToList
                var results = db.camaras.ToList();

                if(GetCamerasCompleted != null)
                {
                    GetCamerasCompleted(this, new FlickCamerasEventArgs(results));
                }
                return; // para que no desacargue, sin sincronización
            }

            string uri = "https://api.flickr.com/services/rest/?method=flickr.cameras.getBrands&api_key=c9ea2337723bf09a027e0258c59c505f&format=rest";

            // el web client permite comunicarse de forma http con algún endpoint
            WebClient client = new WebClient();

            client.DownloadStringCompleted += (s, a) =>
                {
                    if (a.Error == null && !a.Cancelled)
                    {
                        var result = a.Result; 

                        
                        // LINQ para XML

                        // xdocument permite interpretar una cadena
                        var doc = XDocument.Parse(result);

                        // en doc ya está el documento en xml

                        // realizamos una consulta para cada uno de los elementos que sea descendiente del elemento brand
                        var query = from c in doc.Descendants("brand")
                                    select new Camera() { Name = c.Attribute("name").Value }; // creamos una nueva cámara y la instanciamos con el nombre de la cámara que está en el archivo de xml

                        // al ejecutar el método ToList regresa una lista de tipo cámara en este caso es así porque se instanciaron elemento de tipo cámara
                        var results = query.ToList();

                        // empezamos  acrear la base de datos,isostore es la ruta de almacenamiento aisaldo
                        var db = new CameraDataContext("isostore:/cameras.sdf");

                        // verificamos si las base de datos no existe
                        if (!db.DatabaseExists())
                        {
                            // si es así entonces la creamos
                            db.CreateDatabase();
                        }

                        db.camaras.InsertAllOnSubmit(results); // insertams en la base de datos
                        db.SubmitChanges(); // guardamos los cambios en la base de datos

                        if (GetCamerasCompleted != null)
                        {
                            GetCamerasCompleted(this, new FlickCamerasEventArgs(results));
                        }
                    }
                };
            client.DownloadStringAsync(new Uri(uri,UriKind.Absolute));
        }
    }
}
