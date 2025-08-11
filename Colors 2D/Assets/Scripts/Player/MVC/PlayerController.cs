using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController
{
    PlayerInputs _inputs;
    InputAction _ability;
    InputAction _movement;
    InputAction _jump;
    InputAction _changeColor;
    InputAction _meshColor;
    InputAction _saveTin;

    public Vector2 dir;

    public PlayerController(PlayerModel model , Player player, Player_Collisions collisions)
    {
        dir = Vector2.zero;
        _inputs = new PlayerInputs();
        BindActions(_inputs);
        BindEvents(player, model , collisions, _inputs);
    }

    void BindActions(PlayerInputs inputs)
    {
        _ability = inputs.Player.Ability;
        _ability.Enable();

        _movement = inputs.Player.Move;
        _movement.Enable();

        _jump = inputs.Player.Jump;
        _jump.Enable();

        _changeColor = inputs.Player.ChangeColor;
        _changeColor.Enable();

        _meshColor = inputs.Player.MergeColor;
        _meshColor.Enable();

        _saveTin = inputs.Player.SaveTin;
        _saveTin.Enable();
    }

    void BindEvents(Player player, PlayerModel model , Player_Collisions collisions , PlayerInputs inputs)
    {
        inputs.Player.Jump.started += model.JumpEvent;
      
        inputs.Player.Ability.performed += model.Ability;

        inputs.Player.ChangeColor.performed += player.ColorChange;

        inputs.Player.MergeColor.performed += player.ColorMergeEvent;

        inputs.Player.SaveTin.performed += collisions.SaveTinEvent;
    }

    public void InputListener(PlayerModel model)
    {
        if (_inputs.Player.Jump.IsPressed()) model.Jump();

        dir = _inputs.Player.Move.ReadValue<Vector2>();
    }

    public Quaternion MouseController(Transform transform , Vector3 mousePos)
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, rotZ);
    }
}
