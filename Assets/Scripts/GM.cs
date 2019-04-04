﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GM : MonoBehaviour
    {
        #region Atributos

        /// <summary>
        /// Manejado general del juego.
        /// </summary>
        public static GM Gm { get; set; }

        /// <summary>
        /// Lista de la compra.
        /// </summary>
        public List<ShopObject> ShopList { get; set; } = new List<ShopObject>();

        /// <summary>
        /// Nombre del objeto de la lista actual.
        /// </summary>
        public ShopObject CurrentObject { get; set; }

        /// <summary>
        /// Lista de objetos seleccionados correctamente.
        /// </summary>
        private List<ShopObject> CorrectList { get; set; } = new List<ShopObject>();

        /// <summary>
        /// Lista de objetos seleccionados erroneamente.
        /// </summary>
        private List<ShopObject> WrongList { get; set; } = new List<ShopObject>();

        #endregion

        #region Eventos

        private void Awake()
        {
            if (Gm == null)
            {
                Gm = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Gm != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        #endregion

        #region Métodos públicos

        /// <summary>
        /// Carga la siguiente escena.
        /// </summary>
        /// <param name="scene">Nombre de la escena a la que se va a cambiar.</param>
        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        /// <summary>
        /// Método para el botón salir del menú.
        /// </summary>
        public void DoExitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Inicializa/Resetea las listas.
        /// </summary>
        public void InitList()
        {
            ShopList.Clear();
            CorrectList.Clear();
            WrongList.Clear();
        }

        /// <summary>
        /// Recoge el siguiente objeto a seleccionar. Si ya no quedan objetos saltará una excepción.
        /// </summary>
        public void NextObject()
        {
            if (ShopList.Count == 0) throw new Exception();

            CurrentObject = ShopList.First();
            ShopList.Remove(CurrentObject);
        }

        /// <summary>
        /// Se ha acertado el objeto con su respectiva tienda.
        /// </summary>
        /// <param name="obj">Objeto acertado.</param>
        public void CorrectShop(ShopObject obj)
        {
            CorrectList.Add(obj);
        }

        /// <summary>
        /// Se ha equivocado con el objeto con otra tienda.
        /// </summary>
        /// <param name="obj">Objeto erróneo.</param>
        public void WrongShop(ShopObject obj)
        {
            WrongList.Add(obj);
        }

        #endregion

    }
}