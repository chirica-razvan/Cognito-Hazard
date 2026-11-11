using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Also acts as a select filter: only a physical poke can press the button, not ray + grip.
public class ButtonBehavior : MonoBehaviour, IXRSelectFilter
{
    public bool canProcess => isActiveAndEnabled;

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        return interactor is XRPokeInteractor;
    }

    void Awake()
    {
        GetComponent<XRBaseInteractable>().selectFilters.Add(this);
    }

    public void PressedDebugLog()
    {
        Debug.Log($"{gameObject.name} a fost apasat");
    }
}
