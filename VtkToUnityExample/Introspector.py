from ErrorObserver import *
from vtk import *
from Pipeline import *
import ctypes
import collections

import time


class Introspector:

	def setupGlobalWarningHandling(self):
		# Redirect output to a file.
		ow = vtkFileOutputWindow()
		ow.SetFileName("log/vtk_errors.txt")
		vtkOutputWindow.SetInstance(ow)

		# And catch errors in the errorObserver.
		self.eo = ErrorObserver()
		ow.AddObserver('ErrorEvent', self.eo)
		ow.AddObserver('WarningEvent', self.eo)


	def __init__(self):
		self.setupGlobalWarningHandling()
		self.classTree = ClassTree(self.eo, "categories.txt", "categoriesMapping.txt")
		self.log = open("python_embed.py.log", "w")
		self.log.write("Intrspector initialized.\n")
		self.log.flush()


	def __del__(self):
		self.log.close()


	def updateVtkObject(self, node):
		node.vtkInstanceCall('Update')


	'''
	Generates a wrapped Python VTK objects in a PipelineObject, which then exposes the 
	methods for introspective calls. It returns the PyObject itself. For the C++ object
	underlying, use the utility function `getAddress`.
	'''
	def createVtkObject(self, objectName):
		return self.classTree.getTreeObjectByName(objectName).createNode()


	def createVtkObjectWithInstance(self, objectName, vtkInstance):
		return self.classTree.getTreeObjectByName(objectName).createNodeWithInstance(vtkInstance)


	# TODO LOW: move to the TreeObject class
	def getVtkObjectDescriptor(self, node):
		cls = node.vtkInstance.__class__
		methods = getSubclassMethods(cls)
		attributes = [m[3:] for m in methods if m.startswith('Set')]
		get_methods = [m for m in [m for m in methods if m.startswith('Get')] if m[3:] in attributes]

		desc = []

		for m in get_methods:
			method = getattr(node.vtkInstance, m)
			try:
				ret = method()
				if not ret == None and isinstance(ret, collections.Sequence) and not isinstance(ret, str):
					mlen = len(ret)
					if mlen > 0:
						mtype = type2name(ret[0]) + str(mlen);
						desc.append((m[3:], mtype))
				else:
					desc.append((m[3:], type2name(ret)))
			except (TypeError):
				pass

		print(desc)
		return desc


	def getVtkObjectAttribute(self, node, attribute):
		self.log = open("python_embed.py.log", "a")
		self.log.write("==getVtkObjectAttribute\n")
		self.log.flush()
		val = node.vtkInstanceCall("Get" + attribute)
		self.log.write("Return value: " + str(val) + "\n")
		self.log.write("Return type: " + str(type(val)) + "\n")
		self.log.flush()
		if type(val) is tuple:
			self.log.write("Re-encoding tuple... ")
			self.log.flush()
			try:
				r = str(val)[1:-1].replace(" ", "")
				self.log.write("Done: " + r + "\n")
				self.log.flush()
				return r
			except Exception as e:
				self.log.write(f"Error: {e}\n")
				self.log.flush()
				raise e
		self.log.write("String encoding OK: " + str(val) + "\n")
		self.log.flush()
		return str(val)


	def setVtkObjectAttribute(self, node, attribute, vtype, newvalue):
		self.log = open("python_embed.py.log", "a")
		self.log.write("==setVtkObjectAttribute\n")
		methodname = "Set" + attribute
		self.log.write("Method name: " + methodname + "\n")
		self.log.write("Format: " + vtype + "\n")
		self.log.write("Value: " + newvalue + "\n")
		self.log.flush()

		vsize = 0

		if len(vtype) > 1:
			vsize = int(vtype[1:])

		self.log.write(f"Tuple size: {vsize} \n")
		self.log.flush()

		vtypec = vtype[0]

		if vtypec == 's' or vtypec == 'S':
			if (vsize > 0):
				values = newvalue.split(",")
				t = ()
				# Dynamically creating the value tuple
				for v in values:
					t = t + (v,)
				node.vtkInstanceCall(methodname, t)
			node.callSetValueStringMethod(methodname, newvalue)
		elif vtypec == 'd' or vtypec == 'D':
			if (vsize > 0):
				values = newvalue.split(",")
				t = ()
				# Dynamically creating the value tuple
				for v in values:
					t = t + (int(v),)
				node.vtkInstanceCall(methodname, t)
			else:
				node.callSetValueIntMethod(methodname, int(newvalue))
		elif vtypec == 'f' or vtypec == 'F':
			if (vsize > 0):
				values = newvalue.split(",")
				t = ()
				# Dynamically creating the value tuple
				for v in values:
					t = t + (float(v),)
				node.vtkInstanceCall(methodname, t)
			else:
				node.callSetValueFloatMethod(methodname, float(newvalue))
		elif vtypec == 'b' or vtypec == 'B':
			if (vsize > 0):
				values = newvalue.split(",")
				t = ()
				# Dynamically creating the value tuple
				for v in values:
					t = t + (bool(v),)
				node.vtkInstanceCall(methodname, t)
			else:
				node.vtkInstanceCall(methodname, bool(value))
		else:
			raise ValueError("Unknown symbol " + vtype)


	def vtkInstanceCall(self, node, methodName, args, **kwargs):
		self.log = open("python_embed.py.log", "a")
		self.log.write("==vtkInstanceCall\n")
		self.log.write("Method name: " + methodName + "\n")
		self.log.write("Arguments: " + str(args) + "\n")
		self.log.flush()
		return node.vtkInstanceCall(methodName, *args, **kwargs)


	def genericCall(self, object, methodName, args, **kwargs):
		print(methodName, "\n", args, "\n", kwargs)
		return getattr(object, methodName)(*args, **kwargs)


	def deleteVtkObject(self, node):
		del node
		

	def outputFormat(self, value):
		print(value)
		if type(value) is tuple:
			return str(value)[1:-1].replace(" ", "")
		return str(value)


def type2name(t):
	if isinstance(t, int):
		return "d"
	elif isinstance(t, float):
		return "f"
	elif isinstance(t, str):
		return "s"
	elif isinstance(t, bool):
		return "b"
	else:
		return None


def is_abstract(cls):
	try:
		_ = cls()
		return False
	except (TypeError, NotImplementedError):
		return True


def getSubclassMethods(cls):
	methods = set(dir(cls()))
	superclasses = [b for b in cls.__bases__ if not is_abstract(b)]
	if superclasses:
		return list(methods.difference(*(dir(base()) for base in superclasses)))
	else:
		return list(methods)
