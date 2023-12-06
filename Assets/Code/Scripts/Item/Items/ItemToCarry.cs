using DonutDiner.FrameworkModule;
using DonutDiner.PlayerModule;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DonutDiner.ItemModule.Items
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemToCarry : MonoBehaviour, IItem
    {
        #region Delegates

        public event Action OnCarrying = delegate { };

        #endregion Delegates

        #region Fields

        [SerializeField] protected ItemObject _item;
        [SerializeField] protected float _isKinematicTimeout = 10.0f;

        public UnityEvent OnPickUp;
        public UnityEvent OnDrop;

        protected Coroutine _coroutine;

        #endregion Fields

        #region Properties

        public ItemObject Root => _item;

        #endregion Properties

        #region Public Methods

        public virtual void Carry()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);

            gameObject.SetActive(true);
            transform.GetComponent<Rigidbody>().isKinematic = true;
            transform.GetComponent<Collider>().enabled = false;
            transform.parent = Player.Hand;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;

            gameObject.layer = LayerMask.NameToLayer(Layer.FirstPersonCamera);

            OnCarrying();
            if (OnPickUp != null) { OnPickUp.Invoke(); }
        }

        public virtual void Drop()
        {
            transform.GetComponent<Rigidbody>().isKinematic = false;
            transform.GetComponent<Collider>().enabled = true;
            transform.parent = null;

            gameObject.layer = LayerMask.NameToLayer(Layer.Item);

            if (OnDrop != null) { OnDrop.Invoke(); return; }

            if (_coroutine != null) StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(SetIsKinematic());
        }

        public virtual void Place(Vector3 position, bool isItemLocked = false)
        {
            transform.GetComponent<Rigidbody>().isKinematic = true;
            transform.GetComponent<Collider>().enabled = !isItemLocked;
            transform.localPosition = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
            transform.position = position;
            transform.parent = null;

            if (OnDrop != null) { OnDrop.Invoke(); }
        }

        public virtual void Take()
        {
            transform.GetComponent<Rigidbody>().isKinematic = true;
            transform.GetComponent<Collider>().enabled = false;
            transform.parent = null;

        }

        #endregion Public Methods

        #region Private Methods

        protected IEnumerator SetIsKinematic()
        {
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.down;

            float deadline = Time.time + _isKinematicTimeout;

            while (rigidbody.velocity != Vector3.zero && Time.time < deadline && transform.position.y > 0)
            {
                yield return new WaitForFixedUpdate();
            }

            rigidbody.isKinematic = true;

            _coroutine = null;

            yield return null;
        }

        #endregion Private Methods
    }
}