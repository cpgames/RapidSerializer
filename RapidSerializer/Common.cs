using System;

namespace cpGames.RapidSerializer
{
    [Flags]
    public enum SerializationMaskType
    {
        Everything = 0,
        Short = 1,
        Public = 2,
        Private = 4
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SerializationMaskAttribute : Attribute
    {
        #region Fields
        private readonly SerializationMaskType _mask;
        #endregion

        #region Properties
        public SerializationMaskType Mask => _mask;
        #endregion

        #region Constructors
        public SerializationMaskAttribute(SerializationMaskType mask)
        {
            _mask = mask;
        }
        #endregion

        #region Methods
        public bool IsMaskValid(SerializationMaskType mask)
        {
            return (_mask & mask) != mask;
        }
        #endregion
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class CpSerializationIgnoreAttribute : Attribute { }
}