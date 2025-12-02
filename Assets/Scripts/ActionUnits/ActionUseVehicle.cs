using UnityEngine;



[System.Serializable]
public class ActionUseVehicleProperties : ActionInteractProperties
{
    public Vehicle m_Vehicle;
    public VehicleInputController m_VehicleInput;
    public Turret m_Turret;
    public HeadLights m_Lights;
    public Rigidbody m_Rigidbody;
}

public class ActionUseVehicle : ActionInteract
{
    [Header("Action Components")]
    [SerializeField] private ThirdPersonCamera m_Camera;
    [SerializeField] private CharacterInputController m_CharacterInputController;
    [SerializeField] private CharacterController m_CharacterController;
    [SerializeField] private CharacterMovement m_CharacterMovement;
    [SerializeField] private GameObject m_VisualModel;

    private bool InVehicle;
    private void Start()
    {
        EventOnStart.AddListener(OnActionStarted);
        EventOnEnd.AddListener(OnActionEnded);
    }
    private void Update()
    {
        if (InVehicle == true)
        {
            IsCanEnd = (Properties as ActionUseVehicleProperties).m_Vehicle.LinearVelocity < 2;
        }
    }
    private void OnDestroy()
    {
        EventOnStart.RemoveListener(OnActionStarted);
        EventOnEnd.RemoveListener(OnActionEnded);
    }
    private void OnActionStarted()
    {
        ActionUseVehicleProperties prop = Properties as ActionUseVehicleProperties;
        InVehicle = true;

        //Camera
        prop.m_VehicleInput.AssignCamera(m_Camera);

        //VehicleInput
        prop.m_VehicleInput.enabled = true;

        //CharacterInput
        m_CharacterInputController.enabled = false;

        //CharacterMovement
        m_CharacterController.enabled = false;
        m_CharacterMovement.enabled = false;

        //VisualModel
        m_VisualModel.transform.localPosition += new Vector3(0, 1000000, 0);

        //Turret
        if (prop.m_Turret != null)
        {
            prop.m_Turret.SetAim(m_Camera.Aim);
            prop.m_Turret.enabled = true;
        }

        //Light
        if (prop.m_Lights != null) prop.m_Lights.SetHeadLightsState(true);

        //SFXVolume
        prop.m_Vehicle.SetVolume(prop.m_Vehicle.DefaultSFXVolumeBase);

        //RigidBody.IsKinematic
        prop.m_Rigidbody.isKinematic = false;

        //Driver
        prop.m_Vehicle.SetDriver(m_CharacterMovement.GetComponent<Destructible>());
    }
    private void OnActionEnded()
    {
        ActionUseVehicleProperties prop = Properties as ActionUseVehicleProperties;
        InVehicle = false;

        //Camera
        m_CharacterInputController.AssignCamera(m_Camera);

        //VehicleInput
        prop.m_VehicleInput.enabled = false;

        //CharacterInput
        m_CharacterInputController.enabled = true;


        //CharacterMovement
        m_Owner.transform.position = prop.InteractTransform.position;
        m_CharacterController.enabled = true;
        m_CharacterMovement.enabled = true;

        //VisualModel
        m_VisualModel.transform.localPosition = new Vector3(0, 0, 0);

        //Turret
        if (prop.m_Turret != null)
        {
            prop.m_Turret.SetAim(null);
            prop.m_Turret.enabled = false;
        }

        //Light
        if (prop.m_Lights != null) prop.m_Lights.SetHeadLightsState(false);

        //SFXVolume
        prop.m_Vehicle.SetVolume(0);

        //RigidBody.IsKinematic
        prop.m_Rigidbody.isKinematic = true;

        //Driver
        prop.m_Vehicle.SetDriver(null); 
    }
}
