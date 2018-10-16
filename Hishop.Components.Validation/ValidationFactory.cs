using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hishop.Components.Validation
{
	public static class ValidationFactory
	{
		private struct ValidatorCacheKey : IEquatable<ValidatorCacheKey>
		{
			private Type sourceType;

			private string ruleset;

			private bool generic;

			public ValidatorCacheKey(Type sourceType, string ruleset, bool generic)
			{
				this.sourceType = sourceType;
				this.ruleset = ruleset;
				this.generic = generic;
			}

			public override int GetHashCode()
			{
				return this.sourceType.GetHashCode() ^ ((this.ruleset != null) ? this.ruleset.GetHashCode() : 0);
			}

			bool IEquatable<ValidatorCacheKey>.Equals(ValidatorCacheKey other)
			{
				if (this.sourceType == other.sourceType && ((this.ruleset == null) ? (other.ruleset == null) : this.ruleset.Equals(other.ruleset)))
				{
					return this.generic == other.generic;
				}
				return false;
			}
		}

		private static IDictionary<ValidatorCacheKey, Validator> attributeOnlyValidatorsCache = new Dictionary<ValidatorCacheKey, Validator>();

		private static object attributeOnlyValidatorsCacheLock = new object();

		private static IDictionary<ValidatorCacheKey, Validator> attributeAndDefaultConfigurationValidatorsCache = new Dictionary<ValidatorCacheKey, Validator>();

		private static object attributeAndDefaultConfigurationValidatorsCacheLock = new object();

		private static IDictionary<ValidatorCacheKey, Validator> defaultConfigurationOnlyValidatorsCache = new Dictionary<ValidatorCacheKey, Validator>();

		private static object defaultConfigurationOnlyValidatorsCacheLock = new object();

		public static void ResetCaches()
		{
			lock (ValidationFactory.attributeOnlyValidatorsCacheLock)
			{
				ValidationFactory.attributeOnlyValidatorsCache.Clear();
			}
			lock (ValidationFactory.attributeAndDefaultConfigurationValidatorsCacheLock)
			{
				ValidationFactory.attributeAndDefaultConfigurationValidatorsCache.Clear();
			}
			lock (ValidationFactory.defaultConfigurationOnlyValidatorsCacheLock)
			{
				ValidationFactory.defaultConfigurationOnlyValidatorsCache.Clear();
			}
		}

		public static Validator<T> CreateValidator<T>()
		{
			return ValidationFactory.CreateValidator<T>(string.Empty, true);
		}

		public static Validator<T> CreateValidator<T>(string ruleset)
		{
			return ValidationFactory.CreateValidator<T>(ruleset, true);
		}

		private static Validator<T> CreateValidator<T>(string ruleset, bool cacheValidator)
		{
			Validator<T> validator = null;
			if (cacheValidator)
			{
				lock (ValidationFactory.attributeAndDefaultConfigurationValidatorsCacheLock)
				{
					ValidatorCacheKey key = new ValidatorCacheKey(typeof(T), ruleset, true);
					Validator validator2 = default(Validator);
					if (ValidationFactory.attributeAndDefaultConfigurationValidatorsCache.TryGetValue(key, out validator2))
					{
						return (Validator<T>)validator2;
					}
					Validator validator3 = ValidationFactory.InnerCreateValidatorFromAttributes(typeof(T), ruleset);
					validator = ValidationFactory.WrapAndInstrumentValidator<T>(validator3);
					ValidationFactory.attributeAndDefaultConfigurationValidatorsCache[key] = validator;
					return validator;
				}
			}
			Validator validator4 = ValidationFactory.InnerCreateValidatorFromAttributes(typeof(T), ruleset);
			return ValidationFactory.WrapAndInstrumentValidator<T>(validator4);
		}

		public static Validator CreateValidator(Type targetType)
		{
			return ValidationFactory.CreateValidator(targetType, string.Empty);
		}

		public static Validator CreateValidator(Type targetType, string ruleset)
		{
			return ValidationFactory.CreateValidator(targetType, ruleset, true);
		}

		private static Validator CreateValidator(Type targetType, string ruleset, bool cacheValidator)
		{
			Validator validator = null;
			if (cacheValidator)
			{
				lock (ValidationFactory.attributeAndDefaultConfigurationValidatorsCacheLock)
				{
					ValidatorCacheKey key = new ValidatorCacheKey(targetType, ruleset, false);
					Validator result = default(Validator);
					if (ValidationFactory.attributeAndDefaultConfigurationValidatorsCache.TryGetValue(key, out result))
					{
						return result;
					}
					Validator validator2 = ValidationFactory.InnerCreateValidatorFromAttributes(targetType, ruleset);
					validator = ValidationFactory.WrapAndInstrumentValidator(validator2);
					ValidationFactory.attributeAndDefaultConfigurationValidatorsCache[key] = validator;
					return validator;
				}
			}
			Validator validator3 = ValidationFactory.InnerCreateValidatorFromAttributes(targetType, ruleset);
			return ValidationFactory.WrapAndInstrumentValidator(validator3);
		}

		public static Validator<T> CreateValidatorFromAttributes<T>()
		{
			return ValidationFactory.CreateValidatorFromAttributes<T>(string.Empty);
		}

		public static Validator<T> CreateValidatorFromAttributes<T>(string ruleset)
		{
			if (ruleset == null)
			{
				throw new ArgumentNullException("ruleset");
			}
			Validator<T> validator = null;
			lock (ValidationFactory.attributeOnlyValidatorsCacheLock)
			{
				ValidatorCacheKey key = new ValidatorCacheKey(typeof(T), ruleset, true);
				Validator validator2 = default(Validator);
				if (ValidationFactory.attributeOnlyValidatorsCache.TryGetValue(key, out validator2))
				{
					return (Validator<T>)validator2;
				}
				Validator validator3 = ValidationFactory.InnerCreateValidatorFromAttributes(typeof(T), ruleset);
				validator = ValidationFactory.WrapAndInstrumentValidator<T>(validator3);
				ValidationFactory.attributeOnlyValidatorsCache[key] = validator;
				return validator;
			}
		}

		public static Validator CreateValidatorFromAttributes(Type targetType, string ruleset)
		{
			if (ruleset == null)
			{
				throw new ArgumentNullException("ruleset");
			}
			Validator validator = null;
			lock (ValidationFactory.attributeOnlyValidatorsCacheLock)
			{
				ValidatorCacheKey key = new ValidatorCacheKey(targetType, ruleset, false);
				Validator result = default(Validator);
				if (ValidationFactory.attributeOnlyValidatorsCache.TryGetValue(key, out result))
				{
					return result;
				}
				Validator validator2 = ValidationFactory.InnerCreateValidatorFromAttributes(targetType, ruleset);
				validator = ValidationFactory.WrapAndInstrumentValidator(validator2);
				ValidationFactory.attributeOnlyValidatorsCache[key] = validator;
				return validator;
			}
		}

		private static Validator InnerCreateValidatorFromAttributes(Type targetType, string ruleset)
		{
			MetadataValidatorBuilder metadataValidatorBuilder = new MetadataValidatorBuilder();
			return metadataValidatorBuilder.CreateValidator(targetType, ruleset);
		}

		private static Validator<T> WrapAndInstrumentValidator<T>(Validator validator)
		{
			return new GenericValidatorWrapper<T>(validator);
		}

		private static Validator WrapAndInstrumentValidator(Validator validator)
		{
			return new ValidatorWrapper(validator);
		}
	}
}
