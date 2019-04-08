﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.Scripts.ShopTypeEnum;

namespace Assets.Scripts
{
    public class LevelSelector : MonoBehaviour
    {
        #region Atributos
        /// <summary>
        /// Expresión regular para quitar los acentos.
        /// </summary>
        private Regex RegNoAccent { get; } = new Regex("[^a-zA-Z0-9 ]");

        #endregion

        #region Eventos

        void Start() { }

        void Update() { }

        #endregion

        #region Métodos públicos

        /// <summary>
        /// Comienza la partida.
        /// </summary>
        public void Play()
        {
            GM.Gm.InitList();
            LoadShopList();
            SceneManager.LoadScene("Level1");
        }

        #endregion

        #region Métodos privados

        /// <summary>
        /// Carga la lista de la compra de los recursos.
        /// </summary>
        private void LoadShopList()
        {
            TextAsset textlist = (TextAsset)Resources.Load(string.Concat("ShopLists/", "Level1"));
            string txt = Encoding.UTF7.GetString(textlist.bytes);
            List<string> list = new List<string>(txt.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()));

            // La lista estará formada por líneas de tipo -> {nombre del objeto}, {nombre de la tienda}
            list.ForEach(obj =>
            {           
                List<string> listObj = new List<string>(obj.Split(',').Select(o => o.Trim()));
                Debug.Log("&&&&&&&&&&&&&&& List : " + listObj[0]);
                if (listObj.Count == 2)
                {
                    GM.Gm.ShopList.Add(new ShopObject(listObj[0], GetShopType(listObj[1])));
                }
            });
            try
            {
                GM.Gm.NextObject();
            }
            catch (Exception) { }

        }

        /// <summary>
        /// Determina el tipo de tienda según el nombre.
        /// </summary>
        /// <param name="type">Nombre de la tienda.</param>
        /// <returns>Tipo de la tienda.</returns>
        private ShopType GetShopType(string type)
        {
            string normalType = type.Normalize(NormalizationForm.FormD);
            string typeWithout = RegNoAccent.Replace(normalType, string.Empty).ToUpper();
            switch (typeWithout)
            {
                case "CARNICERIA":
                    return ShopType.CARNICERIA;
                case "ELECTRODOMESTICOS":
                    return ShopType.ELECTRODOMESTICOS;
                case "FARMACIA":
                    return ShopType.FARMACIA;
                case "PANADERIA":
                    return ShopType.PANADERIA;
                case "FRUTERIA":
                    return ShopType.FRUTERIA;
                case "PASTELERIA":
                    return ShopType.PASTELERIA;
                default:
                    return ShopType.PESCADERIA;
            }
        }

        #endregion

    }
}
