namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// The default effect used by SdfShapeBatch.
    /// </summary>
    internal class SdfShapeEffect : Effect
    {
        #region Effect Parameters

        EffectParameter viewProjectionParam;

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets the view projection matrix.
        /// </summary>
        public Matrix ViewProjection
        {
            get { return viewProjectionParam.GetValueMatrix(); }
            set { viewProjectionParam.SetValue(value); }
        }


        /// <summary>
        /// Creates a new ShapeEffect.
        /// </summary>
        public SdfShapeEffect(GraphicsDevice device)
            : base(device, Resources.SdfShapeEffect)
        {
            CacheEffectParameters();
        }


        /// <summary>
        /// Creates a new ShapeEffect by cloning parameter settings from an existing instance.
        /// </summary>
        protected SdfShapeEffect(SdfShapeEffect cloneSource)
            : base(cloneSource)
        {
            CacheEffectParameters();
        }


        /// <summary>
        /// Creates a clone of the current ShapeEffect instance.
        /// </summary>
        public override Effect Clone()
        {
            return new SdfShapeEffect(this);
        }


        /// <summary>
        /// Looks up shortcut references to our effect parameters.
        /// </summary>
        void CacheEffectParameters()
        {
            viewProjectionParam = Parameters["view_projection"];
        }

        #endregion
    }
}

