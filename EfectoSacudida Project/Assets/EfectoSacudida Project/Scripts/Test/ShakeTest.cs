//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// CamaraShake.cs (25/02/2017)													\\
// Autor: Antonio Mateo (Moon Pincho) 									        \\
// Descripcion:		Test de efecto Shake            							\\
// Fecha Mod:		25/02/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
#endregion

namespace MoonPincho.Shake
{
    /// <summary>
    /// <para>Test de efecto Shake</para>
    /// </summary>
    public class ShakeTest : MonoBehaviour
    {
        #region Variables
        public CamaraShake.Propiedades test;
        #endregion

        #region Unity Metodos
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FindObjectOfType<CamaraShake>().InitShake(test);
            }
        }
        #endregion
    }
}
