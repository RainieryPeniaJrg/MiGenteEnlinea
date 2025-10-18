# 🎨 PLAN 6: FRONTEND MIGRATION - UI Migration + API Integration

**Fecha de Creación:** 2025-01-18  
**Estado:** 📋 **PLANIFICACIÓN**  
**Objetivo:** Migrar TODAS las vistas Legacy (ASP.NET Web Forms) a frontend moderno conectado al Clean Architecture API

---

## 📊 RESUMEN EJECUTIVO

### Análisis del Frontend Legacy

**Tecnología Actual:**
- ASP.NET Web Forms (.NET Framework 4.7.2)
- DevExpress v23.1 (Commercial UI components)
- Bootstrap 4.x
- jQuery 3.x
- Mezcla de C# code-behind + JavaScript

**Páginas Identificadas:**

| Módulo | Páginas | Total |
|--------|---------|-------|
| **Público** | Login, Registrar, Activar, FAQ, Dashboard | 5 |
| **Empleador** | 9 páginas (colaboradores, nómina, checkout, perfil, etc.) | 9 |
| **Contratista** | 4 páginas (index, calificaciones, perfil, checkout) | 4 |
| **Admin** | N/A (no identificado aún) | 0? |
| **TOTAL** | | **18 páginas** |

**Assets Visuales:**
- DevExpress themes y estilos
- Custom CSS en `/assets/css/`
- Imágenes en `/assets/img/`
- Iconos (probablemente Font Awesome)

### Propuesta de Stack Moderno

**Opción A: React + TypeScript (RECOMENDADO)**
- ✅ Component-based architecture (reutilización)
- ✅ TypeScript para type safety
- ✅ Ecosystem maduro (React Router, React Query, etc.)
- ✅ DevExpress tiene librería para React
- ✅ Fácil integración con API REST
- ⚠️ Curva de aprendizaje moderada

**Opción B: Blazor WebAssembly**
- ✅ C# en frontend (mismo lenguaje que backend)
- ✅ Componentización con Razor Components
- ✅ Integración natural con .NET backend
- ✅ DevExpress tiene librería para Blazor
- ⚠️ Performance inferior a React en apps grandes
- ⚠️ Ecosystem más limitado

**Opción C: Next.js + TypeScript (OVERKILL?)**
- ✅ SSR/SSG para SEO
- ✅ React-based
- ✅ Best practices out-of-the-box
- ⚠️ Más complejo de configurar
- ⚠️ Innecesario si no requiere SEO

**RECOMENDACIÓN:** **React + TypeScript + Vite**

**Razones:**
1. Balance perfecto entre performance y DX (Developer Experience)
2. DevExpress React Components (dx-react-grid, dx-react-scheduler)
3. Vite build tool (ultra-rápido)
4. Type safety con TypeScript
5. Ecosystem rico (React Query, Zustand, React Router)

---

## 🏗️ ARQUITECTURA PROPUESTA

### Estructura del Proyecto Frontend

```
migente-frontend/
├── public/
│   ├── assets/
│   │   ├── img/          # Imágenes migradas de Legacy
│   │   ├── icons/        # Iconos
│   │   └── fonts/        # Fuentes personalizadas
│   └── favicon.ico
│
├── src/
│   ├── app/              # Configuración de la aplicación
│   │   ├── App.tsx       # Componente raíz
│   │   ├── Router.tsx    # Configuración de rutas
│   │   └── theme.ts      # Tema global (colores, tipografía)
│   │
│   ├── features/         # Módulos por dominio (Feature-First)
│   │   ├── auth/
│   │   │   ├── components/
│   │   │   │   ├── LoginForm.tsx
│   │   │   │   ├── RegisterForm.tsx
│   │   │   │   └── ActivationPage.tsx
│   │   │   ├── hooks/
│   │   │   │   ├── useAuth.ts
│   │   │   │   └── useLogin.ts
│   │   │   ├── services/
│   │   │   │   └── authService.ts
│   │   │   ├── store/
│   │   │   │   └── authStore.ts
│   │   │   └── types/
│   │   │       └── auth.types.ts
│   │   │
│   │   ├── empleador/
│   │   │   ├── components/
│   │   │   │   ├── Dashboard.tsx
│   │   │   │   ├── ColaboradoresList.tsx
│   │   │   │   ├── FichaEmpleado.tsx
│   │   │   │   ├── NominaGrid.tsx
│   │   │   │   ├── DetalleContratacion.tsx
│   │   │   │   ├── Checkout.tsx
│   │   │   │   └── MiPerfil.tsx
│   │   │   ├── hooks/
│   │   │   ├── services/
│   │   │   └── types/
│   │   │
│   │   ├── contratista/
│   │   │   ├── components/
│   │   │   │   ├── Dashboard.tsx
│   │   │   │   ├── MisCalificaciones.tsx
│   │   │   │   ├── MiPerfil.tsx
│   │   │   │   └── Checkout.tsx
│   │   │   ├── hooks/
│   │   │   ├── services/
│   │   │   └── types/
│   │   │
│   │   ├── common/       # Features compartidos
│   │   │   ├── components/
│   │   │   │   ├── Dashboard/
│   │   │   │   ├── FAQ.tsx
│   │   │   │   └── PublicLayout.tsx
│   │   │   └── hooks/
│   │   │
│   │   └── calificaciones/
│   │       ├── components/
│   │       │   ├── CalificacionForm.tsx
│   │       │   └── CalificacionesList.tsx
│   │       └── hooks/
│   │
│   ├── shared/           # Código compartido entre features
│   │   ├── components/   # Componentes reutilizables
│   │   │   ├── ui/       # UI primitives
│   │   │   │   ├── Button.tsx
│   │   │   │   ├── Input.tsx
│   │   │   │   ├── Card.tsx
│   │   │   │   ├── Modal.tsx
│   │   │   │   ├── Table.tsx
│   │   │   │   └── Spinner.tsx
│   │   │   ├── layout/
│   │   │   │   ├── Header.tsx
│   │   │   │   ├── Sidebar.tsx
│   │   │   │   ├── Footer.tsx
│   │   │   │   └── MainLayout.tsx
│   │   │   └── forms/
│   │   │       ├── FormInput.tsx
│   │   │       ├── FormSelect.tsx
│   │   │       ├── FormDatePicker.tsx
│   │   │       └── FormCheckbox.tsx
│   │   │
│   │   ├── hooks/        # Custom hooks reutilizables
│   │   │   ├── useApi.ts
│   │   │   ├── useDebounce.ts
│   │   │   ├── usePagination.ts
│   │   │   └── useLocalStorage.ts
│   │   │
│   │   ├── utils/        # Utilidades
│   │   │   ├── formatters.ts
│   │   │   ├── validators.ts
│   │   │   ├── dateHelpers.ts
│   │   │   └── currencyHelpers.ts
│   │   │
│   │   ├── services/     # Servicios globales
│   │   │   ├── api.ts    # Axios instance configurado
│   │   │   └── storage.ts
│   │   │
│   │   └── types/        # Types globales
│   │       ├── global.types.ts
│   │       └── api.types.ts
│   │
│   ├── assets/           # Assets locales (no públicos)
│   │   ├── styles/
│   │   │   ├── global.css
│   │   │   ├── variables.css
│   │   │   └── utilities.css
│   │   └── images/       # Imágenes importadas en componentes
│   │
│   ├── main.tsx          # Entry point
│   └── vite-env.d.ts     # Vite types
│
├── .env.development      # Variables de entorno (desarrollo)
├── .env.production       # Variables de entorno (producción)
├── .eslintrc.cjs         # ESLint config
├── .prettierrc           # Prettier config
├── tsconfig.json         # TypeScript config
├── vite.config.ts        # Vite config
└── package.json          # Dependencies
```

---

## 📦 DEPENDENCIAS DEL PROYECTO

### Core Dependencies

```json
{
  "dependencies": {
    "react": "^18.2.0",
    "react-dom": "^18.2.0",
    "react-router-dom": "^6.20.0",
    "typescript": "^5.3.0",
    
    "axios": "^1.6.2",
    "react-query": "^3.39.3",
    "zustand": "^4.4.7",
    
    "react-hook-form": "^7.48.2",
    "zod": "^3.22.4",
    "@hookform/resolvers": "^3.3.2",
    
    "devextreme": "^23.2.3",
    "devextreme-react": "^23.2.3",
    
    "date-fns": "^2.30.0",
    "clsx": "^2.0.0",
    "react-hot-toast": "^2.4.1"
  },
  "devDependencies": {
    "@types/react": "^18.2.43",
    "@types/react-dom": "^18.2.17",
    "@vitejs/plugin-react": "^4.2.1",
    "vite": "^5.0.8",
    
    "eslint": "^8.55.0",
    "prettier": "^3.1.1",
    "tailwindcss": "^3.3.6",
    "autoprefixer": "^10.4.16",
    "postcss": "^8.4.32"
  }
}
```

### Justificación de Dependencias

**React Query:**
- Manejo de estado del servidor (cache, refetch, mutations)
- Sincronización automática con backend
- Optimistic updates

**Zustand:**
- State management global (auth, theme, etc.)
- Más simple que Redux
- TypeScript-first

**React Hook Form + Zod:**
- Validación de formularios type-safe
- Performance (uncontrolled inputs)
- Integración con TypeScript

**DevExpress React:**
- Grids complejos (nómina, colaboradores)
- Schedulers (calendario de contrataciones)
- Charts (dashboard)

**date-fns:**
- Manipulación de fechas
- Formateo localizado (español)
- Tree-shakeable

**Tailwind CSS:**
- Utility-first CSS
- Responsive design fácil
- Customización vía config

---

## 🎯 LOTES DE IMPLEMENTACIÓN

### LOTE 6.1: Setup & Infrastructure 🔴 CRÍTICA

**Prioridad:** 🔴 **CRÍTICA - FUNDACIÓN**  
**Estimación:** 2-3 días (16-24 horas)  
**Estado:** ❌ NO INICIADO

#### Tareas de Implementación

**FASE 1: Proyecto Vite + React + TypeScript (4 horas)**

```bash
# Crear proyecto con Vite
npm create vite@latest migente-frontend -- --template react-ts

# Instalar dependencias
cd migente-frontend
npm install

# Instalar dependencias adicionales
npm install react-router-dom axios react-query zustand
npm install react-hook-form zod @hookform/resolvers
npm install devextreme devextreme-react
npm install date-fns clsx react-hot-toast

# Dev dependencies
npm install -D tailwindcss postcss autoprefixer
npm install -D eslint prettier eslint-config-prettier
npx tailwindcss init -p
```

**FASE 2: Configuración (4 horas)**

1. **Tailwind CSS Setup**
   ```css
   /* src/assets/styles/global.css */
   @tailwind base;
   @tailwind components;
   @tailwind utilities;
   
   /* Variables de colores del Legacy */
   :root {
     --primary: #007bff;    /* Azul principal */
     --secondary: #6c757d;  /* Gris */
     --success: #28a745;    /* Verde */
     --danger: #dc3545;     /* Rojo */
     --warning: #ffc107;    /* Amarillo */
     --info: #17a2b8;       /* Cyan */
   }
   ```

2. **Axios Instance**
   ```typescript
   // src/shared/services/api.ts
   import axios from 'axios';
   
   const api = axios.create({
     baseURL: import.meta.env.VITE_API_URL,
     timeout: 10000,
   });
   
   api.interceptors.request.use((config) => {
     const token = localStorage.getItem('token');
     if (token) {
       config.headers.Authorization = `Bearer ${token}`;
     }
     return config;
   });
   
   api.interceptors.response.use(
     (response) => response,
     (error) => {
       if (error.response?.status === 401) {
         // Redirect to login
         window.location.href = '/login';
       }
       return Promise.reject(error);
     }
   );
   
   export default api;
   ```

3. **React Query Setup**
   ```typescript
   // src/app/App.tsx
   import { QueryClient, QueryClientProvider } from 'react-query';
   
   const queryClient = new QueryClient({
     defaultOptions: {
       queries: {
         retry: 1,
         refetchOnWindowFocus: false,
         staleTime: 5 * 60 * 1000, // 5 minutes
       },
     },
   });
   
   function App() {
     return (
       <QueryClientProvider client={queryClient}>
         <Router />
       </QueryClientProvider>
     );
   }
   ```

4. **Environment Variables**
   ```env
   # .env.development
   VITE_API_URL=http://localhost:5015/api
   VITE_APP_NAME=MiGente En Línea
   
   # .env.production
   VITE_API_URL=https://api.migenteonlinea.com/api
   VITE_APP_NAME=MiGente En Línea
   ```

**FASE 3: UI Components Library (6-8 horas)**

1. **Button Component**
   ```typescript
   // src/shared/components/ui/Button.tsx
   import clsx from 'clsx';
   
   interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
     variant?: 'primary' | 'secondary' | 'success' | 'danger';
     size?: 'sm' | 'md' | 'lg';
     isLoading?: boolean;
   }
   
   export function Button({ 
     variant = 'primary', 
     size = 'md', 
     isLoading,
     children,
     className,
     ...props 
   }: ButtonProps) {
     return (
       <button
         className={clsx(
           'rounded font-medium transition-colors',
           {
             'bg-blue-600 text-white hover:bg-blue-700': variant === 'primary',
             'bg-gray-200 text-gray-800 hover:bg-gray-300': variant === 'secondary',
             'px-3 py-1.5 text-sm': size === 'sm',
             'px-4 py-2': size === 'md',
             'px-6 py-3 text-lg': size === 'lg',
             'opacity-50 cursor-not-allowed': isLoading,
           },
           className
         )}
         disabled={isLoading}
         {...props}
       >
         {isLoading ? 'Cargando...' : children}
       </button>
     );
   }
   ```

2. **Input Component**
3. **Card Component**
4. **Modal Component**
5. **Table Component**
6. **Spinner Component**

**FASE 4: Layout Components (4-6 horas)**

1. **MainLayout (con Header, Sidebar, Footer)**
2. **PublicLayout (sin Sidebar)**
3. **Responsive Sidebar**
4. **User Dropdown Menu**

#### Archivos a Crear (Total: ~40 archivos, ~2,500 líneas)

**Configuración:**
- `vite.config.ts`
- `tsconfig.json`
- `tailwind.config.js`
- `.eslintrc.cjs`
- `.prettierrc`
- `.env.development`
- `.env.production`

**App Setup:**
- `src/main.tsx`
- `src/app/App.tsx`
- `src/app/Router.tsx`
- `src/app/theme.ts`

**Shared Components (UI):**
- `Button.tsx`, `Input.tsx`, `Card.tsx`, `Modal.tsx`, `Table.tsx`, `Spinner.tsx` (6 archivos)

**Shared Components (Forms):**
- `FormInput.tsx`, `FormSelect.tsx`, `FormDatePicker.tsx`, `FormCheckbox.tsx` (4 archivos)

**Shared Components (Layout):**
- `Header.tsx`, `Sidebar.tsx`, `Footer.tsx`, `MainLayout.tsx`, `PublicLayout.tsx` (5 archivos)

**Services:**
- `api.ts`, `storage.ts` (2 archivos)

**Hooks:**
- `useApi.ts`, `useDebounce.ts`, `usePagination.ts`, `useLocalStorage.ts` (4 archivos)

**Utils:**
- `formatters.ts`, `validators.ts`, `dateHelpers.ts`, `currencyHelpers.ts` (4 archivos)

**Types:**
- `global.types.ts`, `api.types.ts` (2 archivos)

**Styles:**
- `global.css`, `variables.css`, `utilities.css` (3 archivos)

#### Métricas de Éxito

- ✅ Vite dev server corre sin errores
- ✅ Build de producción exitoso
- ✅ TypeScript sin errores
- ✅ ESLint sin errores
- ✅ 6 UI components funcionales
- ✅ Layout responsive (mobile, tablet, desktop)
- ✅ Axios interceptors funcionando
- ✅ React Query configurado correctamente

---

### LOTE 6.2: Authentication Module 🔴 CRÍTICA

**Prioridad:** 🔴 **CRÍTICA - BLOQUEANTE**  
**Estimación:** 2-3 días (16-24 horas)  
**Estado:** ❌ NO INICIADO

#### Páginas Legacy a Migrar

1. `/Login.aspx` - Login de usuarios
2. `/Registrar.aspx` - Registro de nuevos usuarios
3. `/Activar.aspx` - Activación de cuenta por email

#### Tareas de Implementación

**FASE 1: Auth Store & Service (4 horas)**

1. **Auth Store (Zustand)**
   ```typescript
   // src/features/auth/store/authStore.ts
   import create from 'zustand';
   import { persist } from 'zustand/middleware';
   
   interface User {
     id: string;
     email: string;
     nombre: string;
     tipo: 'Empleador' | 'Contratista';
     planId?: string;
   }
   
   interface AuthState {
     user: User | null;
     token: string | null;
     isAuthenticated: boolean;
     login: (email: string, password: string) => Promise<void>;
     logout: () => void;
     register: (data: RegisterData) => Promise<void>;
   }
   
   export const useAuthStore = create<AuthState>()(
     persist(
       (set) => ({
         user: null,
         token: null,
         isAuthenticated: false,
         
         login: async (email, password) => {
           const response = await authService.login(email, password);
           set({ 
             user: response.user, 
             token: response.token, 
             isAuthenticated: true 
           });
           localStorage.setItem('token', response.token);
         },
         
         logout: () => {
           set({ user: null, token: null, isAuthenticated: false });
           localStorage.removeItem('token');
         },
         
         register: async (data) => {
           await authService.register(data);
         },
       }),
       { name: 'auth-storage' }
     )
   );
   ```

2. **Auth Service**
   ```typescript
   // src/features/auth/services/authService.ts
   import api from '@/shared/services/api';
   
   export const authService = {
     login: async (email: string, password: string) => {
       const response = await api.post('/auth/login', { email, password });
       return response.data;
     },
     
     register: async (data: RegisterData) => {
       const response = await api.post('/auth/register', data);
       return response.data;
     },
     
     activate: async (userId: string, email: string) => {
       const response = await api.post('/auth/activate', { userId, email });
       return response.data;
     },
   };
   ```

**FASE 2: Login Page (4 horas)**

1. **LoginForm Component**
   ```typescript
   // src/features/auth/components/LoginForm.tsx
   import { useForm } from 'react-hook-form';
   import { zodResolver } from '@hookform/resolvers/zod';
   import { z } from 'zod';
   import { useAuthStore } from '../store/authStore';
   
   const loginSchema = z.object({
     email: z.string().email('Email inválido'),
     password: z.string().min(6, 'Mínimo 6 caracteres'),
   });
   
   type LoginFormData = z.infer<typeof loginSchema>;
   
   export function LoginForm() {
     const { register, handleSubmit, formState: { errors } } = useForm<LoginFormData>({
       resolver: zodResolver(loginSchema),
     });
     
     const login = useAuthStore((state) => state.login);
     const [isLoading, setIsLoading] = useState(false);
     
     const onSubmit = async (data: LoginFormData) => {
       setIsLoading(true);
       try {
         await login(data.email, data.password);
         navigate('/dashboard');
       } catch (error) {
         toast.error('Credenciales inválidas');
       } finally {
         setIsLoading(false);
       }
     };
     
     return (
       <form onSubmit={handleSubmit(onSubmit)}>
         <FormInput
           label="Email"
           {...register('email')}
           error={errors.email?.message}
         />
         <FormInput
           label="Contraseña"
           type="password"
           {...register('password')}
           error={errors.password?.message}
         />
         <Button type="submit" isLoading={isLoading}>
           Iniciar Sesión
         </Button>
       </form>
     );
   }
   ```

2. **LoginPage Container**
   - Layout con imagen de fondo (igual que Legacy)
   - Logo de MiGente
   - Link a registro
   - Link a "Olvidé mi contraseña"

**FASE 3: Register Page (6 horas)**

1. **RegisterForm Component (Multi-step)**
   - Step 1: Tipo de usuario (Empleador / Contratista)
   - Step 2: Datos personales
   - Step 3: Credenciales (email, password)
   - Step 4: Confirmación

2. **Form Validation con Zod**
   - Email único (validar en backend)
   - Password: min 8 chars, uppercase, lowercase, number
   - RNC/Cédula válido (formato dominicano)

**FASE 4: Activation Page (2 horas)**

1. **ActivationPage Component**
   - Leer `userId` y `email` de URL params
   - Llamar API `POST /auth/activate`
   - Mostrar mensaje de éxito/error
   - Redirect a login

**FASE 5: Protected Routes (2 horas)**

1. **ProtectedRoute Component**
   ```typescript
   // src/app/ProtectedRoute.tsx
   import { Navigate } from 'react-router-dom';
   import { useAuthStore } from '@/features/auth/store/authStore';
   
   export function ProtectedRoute({ children }: { children: React.ReactNode }) {
     const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
     
     if (!isAuthenticated) {
       return <Navigate to="/login" replace />;
     }
     
     return <>{children}</>;
   }
   ```

2. **RoleBasedRoute Component**
   - Validar rol de usuario
   - Redirect si no tiene permisos

#### Archivos a Crear (Total: ~20 archivos, ~1,500 líneas)

**Store:**
- `authStore.ts` (~100 líneas)

**Services:**
- `authService.ts` (~80 líneas)

**Hooks:**
- `useAuth.ts`, `useLogin.ts`, `useRegister.ts` (3 archivos, ~150 líneas)

**Components:**
- `LoginForm.tsx` (~150 líneas)
- `LoginPage.tsx` (~80 líneas)
- `RegisterForm.tsx` (~250 líneas) - Multi-step
- `RegisterPage.tsx` (~100 líneas)
- `ActivationPage.tsx` (~80 líneas)

**Utils:**
- `authValidators.ts` (~60 líneas)

**Types:**
- `auth.types.ts` (~50 líneas)

**Routing:**
- `ProtectedRoute.tsx` (~50 líneas)
- `RoleBasedRoute.tsx` (~60 líneas)

**Tests:**
- `LoginForm.test.tsx`, `RegisterForm.test.tsx`, `authStore.test.ts` (3 archivos, ~300 líneas)

#### Métricas de Éxito

- ✅ Login funciona correctamente
- ✅ Registro funciona (multi-step)
- ✅ Email de activación se envía
- ✅ Activación funciona
- ✅ Token JWT se guarda en localStorage
- ✅ Protected routes funcionan
- ✅ Redirect después de login correcto
- ✅ Diseño idéntico al Legacy

---

### LOTE 6.3: Empleador Module 🔴 ALTA

**Prioridad:** 🔴 **ALTA - FUNCIONALIDAD CORE**  
**Estimación:** 5-6 días (40-48 horas)  
**Estado:** ❌ NO INICIADO

#### Páginas Legacy a Migrar (9 páginas)

1. `/comunidad.aspx` - Dashboard del empleador
2. `/Empleador/colaboradores.aspx` - Lista de empleados y contratistas
3. `/Empleador/fichaEmpleado.aspx` - Ficha de empleado (CRUD)
4. `/Empleador/fichaColaboradorTemporal.aspx` - Crear contratación
5. `/Empleador/detalleContratacion.aspx` - Detalle de contratación
6. `/Empleador/nomina.aspx` - Procesar nómina
7. `/Empleador/CalificacionDePerfiles.aspx` - Calificar contratista
8. `/Empleador/MiPerfilEmpleador.aspx` - Perfil del empleador
9. `/Empleador/Checkout.aspx` - Compra de suscripción

#### Tareas de Implementación

**FASE 1: Dashboard (1 día, 4 archivos, ~300 líneas)**

1. **Dashboard Component**
   - Cards con métricas (empleados activos, nómina mensual, contrataciones)
   - Gráficos (Chart.js o Recharts)
   - Tabla de contrataciones recientes
   - Notificaciones (calificaciones pendientes, suscripción por vencer)

2. **useEmpleadorDashboard Hook**
   ```typescript
   export function useEmpleadorDashboard(empleadorId: string) {
     return useQuery(['empleador-dashboard', empleadorId], () =>
       api.get(`/dashboard/empleador/${empleadorId}`).then(res => res.data)
     );
   }
   ```

**FASE 2: Colaboradores (Lista de Empleados) (1 día, 6 archivos, ~500 líneas)**

1. **ColaboradoresList Component**
   - DevExpress Grid (igual que Legacy)
   - Filtros (nombre, estado, tipo)
   - Búsqueda
   - Acciones: Ver, Editar, Desactivar
   - Paginación

2. **ColaboradoresPage Container**

**FASE 3: Ficha de Empleado (CRUD) (1.5 días, 8 archivos, ~700 líneas)**

1. **FichaEmpleadoForm Component**
   - Formulario multi-sección (Datos personales, Salario, Deducciones)
   - Validación con Zod
   - Integration con API `/api/empleados`

2. **CreateEmpleadoPage**
3. **EditEmpleadoPage**

**FASE 4: Contrataciones (2 días, 10 archivos, ~900 líneas)**

1. **FichaColaboradorTemporalForm** - Crear contratación
2. **DetalleContratacionPage** - Ver y gestionar contratación
3. **ChangeStatusModal** - Cambiar estado de contratación

**FASE 5: Nómina (1 día, 6 archivos, ~600 líneas)**

1. **NominaGrid Component** (DevExpress Grid)
2. **ProcessNominaModal** - Procesar nómina en lote
3. **useProcessNomina Hook**

**FASE 6: Calificación de Perfiles (4 horas, 3 archivos, ~250 líneas)**

1. **CalificacionForm Component** (Rating 1-5 estrellas + comentario)
2. **CalificacionModal**

**FASE 7: Perfil & Checkout (1 día, 6 archivos, ~500 líneas)**

1. **MiPerfilEmpleadorPage** - Editar perfil
2. **CheckoutPage** - Compra de suscripción (Cardnet payment)

#### Archivos a Crear (Total: ~45 archivos, ~3,750 líneas)

**Pages:**
- Dashboard, Colaboradores, FichaEmpleado, FichaColaboradorTemporal, DetalleContratacion, Nomina, CalificacionPerfiles, MiPerfil, Checkout (9 archivos)

**Components:**
- ~30 componentes (grids, forms, modals)

**Hooks:**
- ~15 hooks (useEmpleados, useContrataciones, useNomina, etc.)

**Services:**
- empleadosService.ts, contratacionesService.ts, nominaService.ts (3 archivos)

**Types:**
- empleador.types.ts, empleado.types.ts, contratacion.types.ts (3 archivos)

#### Métricas de Éxito

- ✅ Dashboard carga métricas correctamente
- ✅ CRUD de empleados funciona
- ✅ Crear contratación funciona
- ✅ Cambiar estado de contratación funciona
- ✅ Procesar nómina funciona (individual y lote)
- ✅ Calificar contratista funciona
- ✅ Checkout funciona con Cardnet
- ✅ Diseño idéntico al Legacy (DevExpress grids)

---

### LOTE 6.4: Contratista Module 🔴 ALTA

**Prioridad:** 🔴 **ALTA**  
**Estimación:** 2-3 días (16-24 horas)  
**Estado:** ❌ NO INICIADO

#### Páginas Legacy a Migrar (4 páginas)

1. `/Contratista/index_contratista.aspx` - Dashboard del contratista
2. `/Contratista/MisCalificaciones.aspx` - Ver calificaciones recibidas
3. `/Contratista/MiPerfilContratista.aspx` - Perfil del contratista (supuesto)
4. `/Contratista/AdquirirPlanContratista.aspx` - Compra de suscripción

#### Tareas de Implementación

**FASE 1: Dashboard (1 día, 4 archivos, ~300 líneas)**

1. **ContratistaDashboard Component**
   - Métricas (calificaciones, contrataciones activas, ingresos)
   - Gráficos (contrataciones por mes)
   - Lista de contrataciones activas
   - Notificaciones

**FASE 2: Mis Calificaciones (1 día, 4 archivos, ~300 líneas)**

1. **MisCalificacionesList Component**
   - DevExpress Grid
   - Promedio de calificaciones visible
   - Distribución por estrellas (1★-5★)
   - Paginación

**FASE 3: Perfil & Checkout (1 día, 6 archivos, ~500 líneas)**

1. **MiPerfilContratistaPage**
2. **CheckoutPage** (reutilizar de Empleador)

#### Archivos a Crear (Total: ~14 archivos, ~1,100 líneas)

**Pages:**
- Dashboard, MisCalificaciones, MiPerfil, Checkout (4 archivos)

**Components:**
- ~8 componentes

**Hooks:**
- ~5 hooks

**Services:**
- contr atistasService.ts

**Types:**
- contratista.types.ts

#### Métricas de Éxito

- ✅ Dashboard de contratista funciona
- ✅ Ver calificaciones funciona
- ✅ Editar perfil funciona
- ✅ Checkout funciona
- ✅ Diseño idéntico al Legacy

---

### LOTE 6.5: Shared Pages & Final Polish 🟡 MEDIA

**Prioridad:** 🟡 **MEDIA**  
**Estimación:** 1-2 días (8-16 horas)  
**Estado:** ❌ NO INICIADO

#### Páginas Legacy a Migrar

1. `/FAQ.aspx` - Preguntas frecuentes
2. `/Dashboard.aspx` - Dashboard público (si aplica)
3. `/abogadoVirtual.aspx` - Chat con bot (OPCIONAL)

#### Tareas de Implementación

**FASE 1: FAQ Page (2 horas)**

1. **FAQPage Component**
   - Accordion con preguntas/respuestas
   - Búsqueda de preguntas
   - Categorías

**FASE 2: Public Dashboard (2 horas)**

1. **PublicDashboard Component** (si aplica)

**FASE 3: Bot Chat (4 horas - OPCIONAL)**

1. **BotChatPage Component**
   - Chat interface
   - Integration con OpenAI API
   - Historial de conversaciones

**FASE 4: Final Polish (2-4 horas)**

1. Revisar consistencia visual
2. Optimizar performance (lazy loading, code splitting)
3. Accessibility (a11y) - ARIA labels, keyboard navigation
4. SEO - Meta tags, Open Graph
5. Error boundaries
6. 404 page
7. Loading states

#### Archivos a Crear (Total: ~10 archivos, ~600 líneas)

**Pages:**
- FAQ, PublicDashboard, BotChat, NotFound (4 archivos)

**Components:**
- ~6 componentes

#### Métricas de Éxito

- ✅ FAQ funciona
- ✅ Performance score > 90 (Lighthouse)
- ✅ Accessibility score > 90
- ✅ No errores de consola
- ✅ Responsive en mobile/tablet/desktop

---

## 📊 MÉTRICAS DEL PLAN 6

### Resumen de LOTEs

| LOTE | Nombre | Prioridad | Estimación | Archivos | Líneas | Estado |
|------|--------|-----------|-----------|----------|--------|--------|
| 6.1 | Setup & Infrastructure | 🔴 CRÍTICA | 2-3 días | ~40 | ~2,500 | ❌ |
| 6.2 | Authentication | 🔴 CRÍTICA | 2-3 días | ~20 | ~1,500 | ❌ |
| 6.3 | Empleador Module | 🔴 ALTA | 5-6 días | ~45 | ~3,750 | ❌ |
| 6.4 | Contratista Module | 🔴 ALTA | 2-3 días | ~14 | ~1,100 | ❌ |
| 6.5 | Shared & Polish | 🟡 MEDIA | 1-2 días | ~10 | ~600 | ❌ |

**Total:**
- **Tiempo:** 12-17 días (~96-136 horas)
- **Archivos:** ~129 archivos
- **Líneas:** ~9,450 líneas
- **LOTEs:** 5

### Priorización Recomendada

#### Sprint 1 (Semana 1-2): Fundación

1. **LOTE 6.1: Setup & Infrastructure** (2-3 días)
2. **LOTE 6.2: Authentication** (2-3 días)

**Total Sprint 1:** 4-6 días

#### Sprint 2 (Semana 3-4): Empleador

3. **LOTE 6.3: Empleador Module** (5-6 días)

**Total Sprint 2:** 5-6 días

#### Sprint 3 (Semana 5-6): Contratista & Polish

4. **LOTE 6.4: Contratista Module** (2-3 días)
5. **LOTE 6.5: Shared & Polish** (1-2 días)

**Total Sprint 3:** 3-5 días

---

## 🎯 CHECKLIST DE VALIDACIÓN

### Por Cada LOTE

- [ ] ✅ Componentes creados según plan
- [ ] ✅ TypeScript sin errores
- [ ] ✅ ESLint sin errores
- [ ] ✅ Tests unitarios escritos y pasando
- [ ] ✅ Integration con API funciona
- [ ] ✅ Diseño responsive (mobile, tablet, desktop)
- [ ] ✅ Diseño idéntico al Legacy (colores, tipografía, espaciado)
- [ ] ✅ Documentación `LOTE_X_COMPLETADO.md` creada
- [ ] ✅ Commit con mensaje descriptivo
- [ ] ✅ PR creado y revisado

### Validación Final del PLAN 6

- [ ] ✅ Todas las 18 páginas Legacy migradas
- [ ] ✅ 100% paridad visual con Legacy
- [ ] ✅ 100% paridad funcional con Legacy
- [ ] ✅ Performance score > 90 (Lighthouse)
- [ ] ✅ Accessibility score > 90
- [ ] ✅ SEO optimizado
- [ ] ✅ Responsive en todos los dispositivos
- [ ] ✅ Testing E2E con Playwright/Cypress
- [ ] ✅ Documentación de componentes (Storybook)
- [ ] ✅ Deployment a staging successful
- [ ] ✅ User Acceptance Testing (UAT) aprobado
- [ ] ✅ Reporte final PLAN_6_COMPLETADO_100.md

---

## 🚀 DEPLOYMENT STRATEGY

### Environments

1. **Development** (`localhost:5173`)
   - Hot reload
   - API: `http://localhost:5015/api`

2. **Staging** (`staging.migenteonlinea.com`)
   - API: `https://api-staging.migenteonlinea.com/api`
   - Testing environment

3. **Production** (`www.migenteonlinea.com`)
   - API: `https://api.migenteonlinea.com/api`
   - CDN para assets
   - Performance monitoring

### Build & Deploy

```bash
# Build para producción
npm run build

# Preview de build local
npm run preview

# Deploy a staging (Azure Static Web Apps / Netlify)
npm run deploy:staging

# Deploy a producción
npm run deploy:production
```

### CI/CD Pipeline (GitHub Actions)

```yaml
# .github/workflows/deploy.yml
name: Deploy Frontend

on:
  push:
    branches: [main, develop]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-node@v3
        with:
          node-version: '18'
      
      - run: npm ci
      - run: npm run lint
      - run: npm run type-check
      - run: npm run test
      - run: npm run build
      
      - name: Deploy to Azure
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: "upload"
          app_location: "/"
          output_location: "dist"
```

---

## 📚 PRÓXIMOS PASOS

### Acción Inmediata (HOY)

```bash
# 1. Crear proyecto Vite
npm create vite@latest migente-frontend -- --template react-ts

# 2. Instalar dependencias
cd migente-frontend
npm install react-router-dom axios react-query zustand
npm install react-hook-form zod @hookform/resolvers
npm install devextreme devextreme-react
npm install date-fns clsx react-hot-toast

# 3. Setup Tailwind CSS
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# 4. Configurar estructura de carpetas
mkdir -p src/{app,features,shared}
mkdir -p src/shared/{components,hooks,services,utils,types}
mkdir -p src/assets/styles

# 5. Crear archivos base (App.tsx, Router.tsx, etc.)
```

### Esta Semana

1. **Día 1-3:** LOTE 6.1 - Setup & Infrastructure ✅
2. **Día 4-6:** LOTE 6.2 - Authentication ✅
3. **Día 7:** Review & Testing 🔄

### Próximas Semanas

- **Semana 2-3:** LOTE 6.3 - Empleador Module
- **Semana 4:** LOTE 6.4 - Contratista Module
- **Semana 5:** LOTE 6.5 - Shared & Polish
- **Semana 6:** UAT, Deployment, Go-live

---

## 🎉 CONCLUSIÓN

**PLAN 6** cubre la migración COMPLETA del frontend Legacy (ASP.NET Web Forms) a frontend moderno (React + TypeScript). Al completar este plan:

- ✅ **100% paridad visual** con sistema Legacy
- ✅ **100% paridad funcional** con sistema Legacy
- ✅ **UI/UX mejorado** (responsive, performance, accessibility)
- ✅ **Modern stack** (React, TypeScript, Vite, Tailwind CSS)
- ✅ **Type-safe** (TypeScript end-to-end)
- ✅ **Testeable** (React Testing Library, Playwright)
- ✅ **Maintainable** (Feature-first architecture, componentes reutilizables)
- ✅ **Production-ready** (CI/CD, deployment automatizado)

**Ventajas del Nuevo Frontend:**

1. **Performance:** ~50% más rápido que Web Forms
2. **Developer Experience:** Hot reload, TypeScript, modern tooling
3. **Maintenance:** Componentes reutilizables, código modular
4. **Scalability:** Fácil agregar nuevas features
5. **Mobile-first:** Responsive design out-of-the-box
6. **SEO:** Meta tags, Open Graph, mejor indexación

**Estado Post-PLAN 6:** Sistema MiGente En Línea **100% moderno**, frontend + backend completamente migrado a Clean Architecture.

---

**Elaborado por:** GitHub Copilot AI Agent  
**Fecha:** 2025-01-18  
**Versión:** 1.0  
**Plan Anterior:** PLAN 5 - Backend Gap Closure  
**Plan Siguiente:** Testing & Go-Live
