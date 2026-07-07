# ---------- Build stage ----------
    FROM node:20-alpine AS build
    WORKDIR /app
    
    # Copy package files first for layer caching
    COPY package*.json ./
    RUN npm ci
    
    # Copy sources and build
    COPY . .
    RUN npm run build
    
    # ---------- Runtime stage ----------
    FROM nginx:alpine AS runtime
    
    # Remove default nginx config
    RUN rm /etc/nginx/conf.d/default.conf
    
    # Copy our config
    COPY nginx.conf /etc/nginx/conf.d/default.conf
    
    # Copy built static files
    COPY --from=build /app/dist /usr/share/nginx/html
    
    EXPOSE 80
    
    CMD ["nginx", "-g", "daemon off;"]