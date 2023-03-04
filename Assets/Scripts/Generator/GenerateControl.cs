using UnityEngine;
using MessagePipe;
using VContainer;

/// <summary>
/// Generator�̐����𐧌䂷��R���|�[�l���g
/// </summary>
public class GenerateControl : MonoBehaviour
{
    int _quantity;

    [Inject]
    IPublisher<GeneratorControl> _publisher;

    public void Add()
    {
        _quantity++;

        if (_quantity >= ActorStatusUIManager.InstantiateMax)
        {
            _publisher.Publish(new GeneratorControl(isGeneratable: false));
        }
    }

    public void Remove()
    {
        _quantity--;

        if (_quantity < ActorStatusUIManager.InstantiateMax)
        {
            _publisher.Publish(new GeneratorControl(isGeneratable: true));
        }
    }
}
