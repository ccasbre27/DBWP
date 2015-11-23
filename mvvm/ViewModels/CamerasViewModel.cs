using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.ViewModels
{
    public class CamerasViewModel : NotificationEnabledObject
    {
        // el ObservableCollection es adecuado para enlaces de datos en las tecnologías xaml
        private ObservableCollection <Camera> cameraList;
        ServiceModel serviceModel = new ServiceModel();

        public CamerasViewModel()
        {
            //DeserializeJSON();
            serviceModel.GetCamerasCompleted += (s, a) =>
                {
                    cameraList = new ObservableCollection<Camera>(a.Results);

                    // llamamos el método para que serialice cada elemento de la lista
                    //SerializeJSON();
                    if(!DesignerProperties.IsInDesignTool)
                    {
                        serviceModel.getCameras();
                    }

                };
        }

        const string fileName = "Datos.json";
        void SerializeJSON()
        {
            // serializar los datos en JSON, en tipo ObservableCollection<Camera>
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Camera>));

            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // verificamos si el archivo existe
                if (isf.FileExists(fileName))
                {
                    isf.DeleteFile(fileName); // si es así entonces lo eliminamos
                }

                 using (var stream = isf.CreateFile(fileName))
                 {
                     // escribimos en ese stream los resultados
                     serializer.WriteObject(stream, cameraList);
                     stream.Close(); // cerramos el stream
                 }
            }

           
        }

        void DeserializeJSON()
        {
             // verificamos si no se está en tiempo de diseño
            if (!DesignerProperties.IsInDesignTool)
            {
                // obtenemos el almacenamiento a
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    // verificamos si el archivo existe
                    if (isf.FileExists(fileName))
                    {
                        // si es así entonces lo vamos a leer

                        using (var stream = isf.OpenFile(fileName,System.IO.FileMode.Open))
                        {
                            // serializar los datos en XML, en tipo ObservableCollection<Camera>
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Camera>));

                            // leemos los datos y los deserializamos
                            var data = serializer.ReadObject(stream) as ObservableCollection<Camera>;

                            // verificamos si data tiene algo
                            if (data != null && data.Count > 0)
                            {
                                // si tiene datos entoces
                                cameraList = data; // establecemos la lista igual a lo que acbamos de leer
                            }
                            stream.Close(); // cerramos el stream
                        }
                    }

                   
                }
            }

        }
        public ObservableCollection <Camera> CameraList
        {
            get 
            { 
                if (cameraList == null)
                {
                    cameraList = new ObservableCollection<Camera>();
                }

                // esto es para agregar elementos y tener blendability, si se está en tiempo de diseño entonces se muestren datos de ejemplo
                if(DesignerProperties.IsInDesignTool)
                {
                    for (int i = 0 ; i < 20 ; i++)
                    {
                        cameraList.Add(new Camera() { Name = "Camara #" });
                    }
                }

                return cameraList; 
            }
            set 
            { 
                cameraList = value;
                OnPropertyChanged();
            }
        }

        ActionCommand getCamerasCommand;
        public ActionCommand GetCamerasCommand
        {
            get 
            { 
                if (getCamerasCommand == null)
                {
                    getCamerasCommand = new ActionCommand(() => 
                    {
                        serviceModel.getCameras();
                    });
                }
                return getCamerasCommand;
            }
            
        }
        
    }
}
