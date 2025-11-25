using UnityEngine;
using UnityEngine.InputSystem;

// Este es un script de depuración para visualizar los rayos del ratón.
// Añádelo a la misma cámara que tiene el Physics Raycaster.
public class RaycastVisualizer : MonoBehaviour
{
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        if (_camera == null)
        {
            Debug.LogError("RaycastVisualizer: ¡No se encontró un componente Camera en este GameObject!");
        }
    }

    void Update()
    {
        if (_camera == null || !_camera.enabled) return;

        // Crear un rayo desde la cámara a la posición del ratón
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        // Dibujar el rayo en la vista de escena para que podamos verlo.
        // La línea será roja y tendrá una longitud de 200 unidades.
        Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red);
        
        // Imprimir en la consola para depuración
        Debug.Log($"RaycastVisualizer activo en la cámara: {_camera.name}. Origen: {ray.origin}, Dirección: {ray.direction}");
    }
}