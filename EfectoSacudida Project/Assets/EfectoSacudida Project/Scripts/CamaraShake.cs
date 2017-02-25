//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// CamaraShake.cs (25/02/2017)													\\
// Autor: Antonio Mateo (Moon Pincho) 									        \\
// Descripcion:		Efecto Shake(Sacudida) de la camara							\\
// Fecha Mod:		25/02/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using System.Collections;
#endregion

namespace MoonPincho.Shake
{
    /// <summary>
    /// <para>Efecto Shake(Sacudida) de la camara</para>
    /// </summary>
    [RequireComponent(typeof(Camera))]
	public class CamaraShake : MonoBehaviour 
	{
        #region Variables
        /// <summary>
        /// <para>Angulo maximo de la camara.</para>
        /// </summary>
        private float anguloMax = 10f;                                              // Angulo maximo de la camara
        /// <summary>
        /// <para>Temporal var para la corutina.</para>
        /// </summary>
        IEnumerator tempShakeCoroutina;                                             // Temporal var para la corutina
        #endregion

        #region Propiedades
        [System.Serializable]
        public class Propiedades
        {
            /// <summary>
            /// <para>Angulo de la camara.</para>
            /// </summary>
            public float angulo;                                                    // Angulo de la camara
            /// <summary>
            /// <para>Fuerza de la camara.</para>
            /// </summary>
            public float fuerza;                                                    // Fuerza de la camara
            /// <summary>
            /// <para>Velocidad del shake.</para>
            /// </summary>
            public float velocidad;                                                 // Velocidad del shake
            /// <summary>
            /// <para>Duracion del shake.</para>
            /// </summary>
            public float duracion;                                                  // Duracion del shake
            /// <summary>
            /// <para>Brusquedad del shake.</para>
            /// </summary>
            [Range(0, 1)]
            public float brusquedad;                                                // Brusquedad del shake
            /// <summary>
            /// <para>Desfase de la camara.</para>
            /// </summary>
            [Range(0, 1)]
            public float desfase;                                                   // Desfase de la camara
            /// <summary>
            /// <para>Rotacion del shake.</para>
            /// </summary>
            [Range(0, 1)]
            public float rotacion;                                                  // Rotacion del shake

            /// <summary>
            /// <para>Propiedades del efecto shake de la camara.</para>
            /// </summary>
            /// <param name="angulo">Angulo de la camara.</param>
            /// <param name="fuerza">Fuerza de la camara.</param>
            /// <param name="velocidad">Velocidad del shake.</param>
            /// <param name="duracion">Duracion del shake.</param>
            /// <param name="brusquedad">Brusquedad del shake.</param>
            /// <param name="desfase">Desfase de la camara.</param>
            /// <param name="rotacion">Rotacion del shake.</param>
            public Propiedades(float angulo, float fuerza, float velocidad, float duracion, float brusquedad, float desfase, float rotacion)// Propiedades del efecto shake de la camara
            {
                this.angulo = angulo;
                this.fuerza = fuerza;
                this.velocidad = velocidad;
                this.duracion = duracion;
                this.brusquedad = Mathf.Clamp01(brusquedad);
                this.desfase = Mathf.Clamp01(desfase);
                this.rotacion = Mathf.Clamp01(rotacion);
            }
        }
        #endregion

        #region Actualizador
        /// <summary>
        /// <para>Efecto shake de la camara.</para>
        /// </summary>
        /// <param name="propiedades">Propiedades del Shake.</param>
        /// <returns></returns>
        IEnumerator Shake(Propiedades propiedades)// Efecto shake de la camara
        {
            #region Variables
            float finalizado = 0;
            float movimiento = 0;
            float radioAngulo = propiedades.angulo * Mathf.Deg2Rad - Mathf.PI;
            Vector3 prevWaypoint = Vector3.zero;
            Vector3 actualWaypoint = Vector3.zero;
            float moveDistancia = 0;
            Quaternion targetRotacion = Quaternion.identity;
            Quaternion prevRotacion = Quaternion.identity;
            #endregion

            #region Funcionalidad
            do
            {
                if (movimiento >= 1 || finalizado == 0)
                {
                    float tempDesfase = CurvaDesfase(finalizado, propiedades.desfase);
                    float anguloBrusquedad = (Random.value - .5f) * Mathf.PI;

                    radioAngulo += Mathf.PI + anguloBrusquedad * propiedades.brusquedad;
                    actualWaypoint = new Vector3(Mathf.Cos(radioAngulo), Mathf.Sin(radioAngulo)) * propiedades.fuerza * tempDesfase;
                    prevWaypoint = transform.localPosition;
                    moveDistancia = Vector3.Distance(actualWaypoint, prevWaypoint);

                    targetRotacion = Quaternion.Euler(new Vector3(actualWaypoint.y, actualWaypoint.x).normalized * propiedades.rotacion * tempDesfase * anguloMax);
                    prevRotacion = transform.localRotation;

                    movimiento = 0;
                }

                finalizado += Time.deltaTime / propiedades.duracion;
                movimiento += Time.deltaTime / moveDistancia * propiedades.velocidad;
                transform.localPosition = Vector3.Lerp(prevWaypoint, actualWaypoint, movimiento);
                transform.localRotation = Quaternion.Slerp(prevRotacion, targetRotacion, movimiento);

                yield return null;
            } while (moveDistancia > 0);
            #endregion
        }
        #endregion

        #region API
        /// <summary>
        /// <para>Inicia el efecto Shake.</para>
        /// </summary>
        /// <param name="properties">Los valores de las propiedades del efecto.</param>
        public void InitShake(Propiedades properties)// Inicia el efecto Shake
        {
            if (tempShakeCoroutina != null)
            {
                StopCoroutine(tempShakeCoroutina);
            }

            tempShakeCoroutina = Shake(properties);
            StartCoroutine(tempShakeCoroutina);
        }
        #endregion

        #region Funcionalidad
        /// <summary>
        /// <para>La curva que hace el desfase.</para>
        /// </summary>
        /// <param name="x">Valor inicial.</param>
        /// <param name="desfase">Valor desfase.</param>
        /// <returns></returns>
        private float CurvaDesfase(float x, float desfase)// La curva que hace el desfase
        {
            x = Mathf.Clamp01(x);
            float a = Mathf.Lerp(2, .25f, desfase);
            float b = 1 - Mathf.Pow(x, a);
            return b * b * b;
        }
        #endregion
    }
}
