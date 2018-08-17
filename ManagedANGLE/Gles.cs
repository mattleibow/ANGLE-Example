using System;

using System.Runtime.InteropServices;

using GLchar = System.Byte;
using GLenum = System.UInt32;
using GLboolean = System.Byte;
using GLbitfield = System.UInt32;
using GLbyte = System.SByte;
using GLshort = System.UInt16;
using GLint = System.Int32;
using GLubyte = System.Byte;
using GLushort = System.UInt16;
using GLuint = System.UInt32;
using GLsizei = System.Int32;
using GLfloat = System.Single;
using GLclampf = System.Single;
using GLdouble = System.Double;
using GLclampd = System.Double;
using GLintptr = System.Int64;
using GLsizeiptr = System.Int64;
using System.Text;

namespace ManagedANGLE
{
    internal static class Gles
    {
        private const string libGLESv2 = "libGLESv2.dll";

        public const GLshort GL_DEPTH_BUFFER_BIT = 0x00000100;
        public const GLshort GL_STENCIL_BUFFER_BIT = 0x00000400;
        public const GLshort GL_COLOR_BUFFER_BIT = 0x00004000;

        public const GLboolean GL_FALSE = 0;
        public const GLboolean GL_TRUE = 1;

        public const GLshort GL_POINTS = 0x0000;
        public const GLshort GL_LINES = 0x0001;
        public const GLshort GL_LINE_LOOP = 0x0002;
        public const GLshort GL_LINE_STRIP = 0x0003;
        public const GLshort GL_TRIANGLES = 0x0004;
        public const GLshort GL_TRIANGLE_STRIP = 0x0005;
        public const GLshort GL_TRIANGLE_FAN = 0x0006;

        public const GLshort GL_ARRAY_BUFFER = 0x8892;
        public const GLshort GL_ELEMENT_ARRAY_BUFFER = 0x8893;

        public const GLshort GL_STATIC_DRAW = 0x88E4;

        public const GLshort GL_COMPILE_STATUS = 0x8B81;
        public const GLshort GL_LINK_STATUS = 0x8B82;

        public const GLshort GL_INFO_LOG_LENGTH = 0x8B84;

        public const GLshort GL_FRAGMENT_SHADER = 0x8B30;
        public const GLshort GL_VERTEX_SHADER = 0x8B31;

        public const GLshort GL_STENCIL_TEST = 0x0B90;
        public const GLshort GL_DEPTH_TEST = 0x0B71;
        public const GLshort GL_SCISSOR_TEST = 0x0C11;

        public const GLshort GL_BYTE = 0x1400;
        public const GLshort GL_UNSIGNED_BYTE = 0x1401;
        public const GLshort GL_SHORT = 0x1402;
        public const GLshort GL_UNSIGNED_SHORT = 0x1403;
        public const GLshort GL_INT = 0x1404;
        public const GLshort GL_UNSIGNED_INT = 0x1405;
        public const GLshort GL_FLOAT = 0x1406;
        public const GLshort GL_FIXED = 0x140C;

        [DllImport(libGLESv2)]
        public static extern GLuint glCreateProgram();

        [DllImport(libGLESv2)]
        public static extern void glDeleteProgram(GLuint program);

        [DllImport(libGLESv2)]
        public static extern void glLinkProgram(GLuint program);

        [DllImport(libGLESv2)]
        public static extern void glGetProgramiv(GLuint program, GLenum pname, out GLint param);

        [DllImport(libGLESv2)]
        public static extern void glUseProgram(GLuint program);

        [DllImport(libGLESv2)]
        public static extern void glGetProgramInfoLog(GLuint program, GLsizei maxLength, out GLsizei length, StringBuilder infoLog);

        [DllImport(libGLESv2)]
        public static extern GLint glGetAttribLocation(GLuint program, String name);

        [DllImport(libGLESv2)]
        public static extern GLint glGetUniformLocation(GLuint program, String name);

        [DllImport(libGLESv2)]
        public static extern void glAttachShader(GLuint program, GLuint shader);

        [DllImport(libGLESv2)]
        public static extern void glGenBuffers(GLsizei n, out GLuint buffers);

        [DllImport(libGLESv2)]
        public static extern void glDeleteBuffers(GLsizei n, ref GLuint buffers);

        [DllImport(libGLESv2)]
        public static extern void glBindBuffer(GLenum target, GLuint buffer);

        [DllImport(libGLESv2)]
        public static extern void glBufferData(GLenum target, GLsizeiptr size, IntPtr data, GLenum usage);

        public static void glBufferData<T>(GLenum target, GLsizeiptr size, T[] data, GLenum usage)
        {
            var gcData = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                glBufferData(target, size * Marshal.SizeOf<T>(), gcData.AddrOfPinnedObject(), usage);
            }
            finally
            {
                gcData.Free();
            }
        }

        [DllImport(libGLESv2)]
        public static extern GLuint glCreateShader(GLenum type);

        [DllImport(libGLESv2)]
        public static extern void glDeleteShader(GLuint shader);

        [DllImport(libGLESv2)]
        public static extern void glShaderSource(GLuint shader, GLsizei count, String[] source, GLint[] length);

        [DllImport(libGLESv2)]
        public static extern void glCompileShader(GLuint shader);

        [DllImport(libGLESv2)]
        public static extern void glGetShaderiv(GLuint shader, GLenum pname, out GLint param);

        [DllImport(libGLESv2)]
        public static extern void glGetShaderInfoLog(GLuint shader, GLsizei maxLength, out GLsizei length, StringBuilder infoLog);

        [DllImport(libGLESv2)]
        public static extern void glViewport(GLint x, GLint y, GLsizei width, GLsizei height);

        [DllImport(libGLESv2)]
        public static extern void glEnable(GLenum cap);

        [DllImport(libGLESv2)]
        public static extern void glClear(GLbitfield mask);

        [DllImport(libGLESv2)]
        public static extern void glClearColor(GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha);

        [DllImport(libGLESv2)]
        public static extern void glDrawElements(GLenum mode, GLsizei count, GLenum type, IntPtr indices);

        [DllImport(libGLESv2)]
        public static extern void glEnableVertexAttribArray(GLuint index);

        [DllImport(libGLESv2)]
        public static extern void glUniformMatrix4fv(GLint location, GLsizei count, GLboolean transpose, GLfloat[,] value);

        [DllImport(libGLESv2)]
        public static extern void glVertexAttribPointer(GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, IntPtr pointer);
    }
}
