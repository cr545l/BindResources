using UnityEngine;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lofle
{
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false )]
	public class Binder : Attribute
	{
		public interface IProperty<T>
		{
			T Value { get; }
		}

		protected readonly object _value;

		public Binder( object value )
		{
			_value = value;
		}
	}

	public class StringBinder : Binder, Binder.IProperty<string>
	{
		public StringBinder( string value ) : base( value ) { }
		public string Value { get { return (string)_value; } }
	}

	public static class BindResources
	{
		public static StringBinder GetBinder( Type type )
		{
			return ( (StringBinder)Attribute.GetCustomAttribute( type, typeof( StringBinder ) ) );
		}

		public static StringBinder GetBinder<T>() where T : UnityEngine.Object
		{
			return ( (StringBinder)Attribute.GetCustomAttribute( typeof( T ), typeof( StringBinder ) ) );
		}

		public static UnityEngine.Object Load( Type type )
		{
			return Resources.Load( GetBinder( type ).Value );
		}

		public static T Load<T>() where T : UnityEngine.Object
		{
			return Resources.Load<T>( GetBinder<T>().Value );
		}

		public static UnityEngine.Object Instantaite( Type type )
		{
			return UnityEngine.Object.Instantiate( Load( type ) );
		}

		public static T Instantaite<T>() where T : UnityEngine.Object
		{
			return UnityEngine.Object.Instantiate( Load<T>() );
		}
	}
}

namespace LofleEditor
{
	public static class Binder
	{
		private const string _MENU_STRING = "Assets/Get Bind Path";

#if UNITY_EDITOR
		[MenuItem( _MENU_STRING, false, 0 )]
		private static void PathClipboard()
		{
			if( 0 < Selection.objects.Length )
			{
				string path = AssetDatabase.GetAssetPath( Selection.objects.FirstOrDefault() );

				path = path.Substring( "Assets/Resources/".Length );
				path = GetFullPathWithoutExtension( path );

#if NET_4_6
				string result = $"[{nameof( Lofle.StringBinder )}( @\"{path}\" )]";
#else
				string result = string.Format( "[Lofle.StringBinder( @\"{0}\" )]", path );
#endif

				EditorGUIUtility.systemCopyBuffer = result;
			}
		}
#endif

		private static String GetFullPathWithoutExtension( String path )
		{
			return System.IO.Path.Combine( System.IO.Path.GetDirectoryName( path ), System.IO.Path.GetFileNameWithoutExtension( path ) );
		}
	}
}
