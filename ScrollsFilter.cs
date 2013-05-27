using System;
using System.Collections;
using System.Collections.Generic;
using LinFu.AOP.Cecil;
using LinFu.AOP.Cecil.Interfaces;

namespace ScrollsFilter
{
	/*
	 * This namespace filters classes and methods, that need to be patched
	 * Future version will obtain names from mods dynamically
	 * 
	 * Also take care of what should be patched and what is not allowed to.
	 * Methods returning primitive types like "bool" are unsafe and may lead to invalid IL code (e.g. Communicator:sendSilentRequest)
	 * TO-DO: Sort out, when this does happen
	 * 			=> block it or better fix LinFu
	 * 
	 */



	public class ScrollsFilter : IMethodFilter, ITypeFilter
	{
		public static List<Mono.Cecil.MethodReference> hooks = new List<Mono.Cecil.MethodReference>();

		public static void AddHook(Mono.Cecil.MethodDefinition method) {
			hooks.Add (new Mono.Cecil.MethodReference(method.Name, method.DeclaringType, method.ReturnType.ReturnType, method.HasThis, method.ExplicitThis, method.CallingConvention));
		}

		public ScrollsFilter ()
		{
		}

		#region IMethodFilter implementation
		public bool ShouldWeave (Mono.Cecil.MethodReference targetMethod)
		{
			if (hooks.Contains(targetMethod))
				return true;
			return false;
			/*if (targetMethod.DeclaringType.Name.Equals ("Communicator") && (targetMethod.Name.Equals ("sendRequest") || targetMethod.Name.Equals("sendSilentRequest"))  && targetMethod.Parameters[0].ParameterType.Name.Equals("String")) {
				Console.WriteLine ("MethodFilter: "+targetMethod.Name);
				return true;
			} else
			    return false;*/
		}
		#endregion

		#region ITypeFilter implementation
		public bool ShouldWeave (Mono.Cecil.TypeReference targetType)
		{
			foreach (Mono.Cecil.MethodReference targetMethod in hooks) {
				if (targetMethod.DeclaringType.Equals (targetType))
					return true;
			}
			return false;
			/*if (targetType.Name.Equals ("Communicator")) {
				Console.WriteLine ("TypeFilter: "+targetType.FullName);
				return true;
			} else
				return false;*/
		}
		#endregion
	}

	//not needed

	/*public class ScrollsMethodCallFilter : IMethodCallFilter
	{
		public ScrollsMethodCallFilter ()
		{
		}

		#region IMethodCallFilter implementation
		public bool ShouldWeave (Mono.Cecil.TypeReference targetType, Mono.Cecil.MethodReference hostMethod, Mono.Cecil.MethodReference currentMethodCall)
		{
			Console.WriteLine ("MethodCallFilter: "+targetType.FullName+" ,"+hostMethod.Name+" ,"+currentMethodCall.Name);
			if (targetType.Name.Equals("Communicator") && (hostMethod.Name.Equals("addListener") || currentMethodCall.Name.Equals("addListener")))
				return true;
			else
				return false;
		}
		#endregion
	}

	public class ScrollsInstFilter : INewInstanceFilter
	{
		public ScrollsInstFilter ()
		{
		}

		#region INewInstanceFilter implementation
		public bool ShouldWeave (Mono.Cecil.MethodReference currentConstructor, Mono.Cecil.TypeReference concreteType, Mono.Cecil.MethodReference hostMethod)
		{
			Console.WriteLine ("NewInstanceFilter: "+currentConstructor.Name+" ,"+concreteType.FullName+" ,"+hostMethod.Name);
			return false;
		}
		#endregion
	}

	public class ScrollsFieldFilter : IFieldFilter
	{
		public ScrollsFieldFilter ()
		{
		}

		#region IFieldFilter implementation
		public bool ShouldWeave (Mono.Cecil.MethodReference hostMethod, Mono.Cecil.FieldReference targetField)
		{
			Console.WriteLine ("FieldFilter: "+hostMethod.Name+" ,"+targetField.Name);
			return false;
		}
		#endregion
	}*/
}

