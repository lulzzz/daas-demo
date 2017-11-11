import { inject, transient, TransientRegistration } from 'aurelia-framework';
import { HttpClient } from 'aurelia-fetch-client';

/**
 * Client for the Database-as-a-Service API.
 */
@transient()
@inject(HttpClient)
export class DaaSAPI
{
    /**
     * Create a new DaaS API client.
     * 
     * @param http An HTTP client.
     */
    constructor(private http: HttpClient)
    {
        http.configure(request =>
            request.withBaseUrl(
                'http://kr-cluster.tintoy.io:31200/api/v1/'
            )
            .withDefaults({
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
        );
    }

    /**
     * Get information about all tenants.
     * 
     * @returns The tenants, sorted by name.
     */
    public async getTenants(): Promise<Tenant[]> {
        const response = await this.http.fetch('tenants');
        const body = await response.json();

        return body as Tenant[];
    }

    /**
     * Get information about a specific tenant.
     * 
     * @param tenantId The tenant Id.
     * @returns The tenant, or null if no tenant exists with the specified Id.
     */
    public async getTenant(tenantId: number): Promise<Tenant | null> {
        const response = await this.http.fetch(`tenants/${tenantId}`);
        const body = await response.json();

        if (response.ok)
            return body as Tenant;

        if (response.status == 400)
        {
            const notFound = body as NotFoundResponse;
            if (notFound.entityType == 'DatabaseServer')
                return null;
        }

        const errorResponse = body as ApiErrorResponse;

        throw new Error(
            `Failed to retrieve details for tenant with Id ${tenantId}: ${errorResponse.message || 'Unknown error.'}`
        );
    }

    /**
     * Get information about a tenant's SQL server instance.
     * 
     * @param tenantId The Id of the tenant that owns the server.
     * @returns The server, or null if the tenant does not have a server.
     */
    public async getTenantServer(tenantId: number): Promise<Server | null> {
        const response = await this.http.fetch(`tenants/${tenantId}/server`);
        const body = await response.json();

        if (response.ok) {
            return body as Server;
        }

        if (response.status == 400)
        {
            const notFound = body as NotFoundResponse;
            if (notFound.entityType == 'DatabaseServer')
                return null;
        }

        const errorResponse = body as ApiErrorResponse;

        throw new Error(
            `Failed to retrieve details for server owned by tenant with Id ${tenantId}: ${errorResponse.message || 'Unknown error.'}`
        );
    }

    /**
     * Get information about a tenant's SQL server instance.
     * 
     * @param tenantId The Id of the tenant that owns the server.
     * @returns The databases, or null if the tenant does not have a server.
     */
    public async getTenantDatabases(tenantId: number): Promise<Database[] | null> {
        const response = await this.http.fetch(`tenants/${tenantId}/databases`);
        const body = await response.json();

        if (response.ok) {
            return body as Database[];
        }

        if (response.status == 400)
        {
            const notFound = body as NotFoundResponse;
            if (notFound.entityType == 'DatabaseServer')
                return null;
        }

        const errorResponse = body as ApiErrorResponse;

        throw new Error(
            `Failed to retrieve databases owned by tenant with Id ${tenantId}: ${errorResponse.message || 'Unknown error.'}`
        );
    }

    /**
     * Delete a tenant's database.
     * 
     * @param tenantId The tenant Id.
     * @param databaseId The database Id.
     */
    public async deleteDatabase(tenantId: number, databaseId: number): Promise<void> {
        const response = await this.http.fetch(`tenants/${tenantId}/databases/${databaseId}`, {
            method: 'DELETE'
        });
        
        if (response.ok || response.status === 400) {
            return
        }

        const body = await response.json();
        const errorResponse = body as ApiErrorResponse;

        throw new Error(
            `Failed to retrieve databases owned by tenant with Id ${tenantId}: ${errorResponse.message || 'Unknown error.'}`
        );
    }
}

/**
 * Represents a DaaS Tenant.
 */
export interface Tenant
{
    /**
     * The tenant Id.
     */
    id: number;
    
    /**
     * The tenant name.
     */
    name: string;
}

/**
 * Represents a DaaS SQL Server instance.
 */
export interface Server
{
    /**
     * The server Id.
     */
    id: number;
    
    /**
     * The server name.
     */
    name: string;

    /**
     * The Id of the tenant that owns the server.
     */
    tenantId: number;

    /**
     * The currently-requested action (if any) for the server.
     */
    action?: string | null;

    /**
     * The server status.
     */
    status?: string | null;

    /**
     * The fully-qualified domain name (if any) on which the server is externally accessible.
     */
    publicFQDN?: string | null;

    /**
     * The TCP port (if any) on which the server is externally accessible.
     */
    publicPort?: number | null;
}

/**
 * Represents an instance of an SQL Server database.
 */
export interface Database {
    /**
     * The database Id.
     */
    id: number;
    
    /**
     * The database name.
     */
    name: string;

    /**
     * The database connection string (if available).
     */
    connectionString: string | null;
}

/**
 * Represents an error from the DaaS API.
 */
interface ApiErrorResponse {
    /**
     * An informational message describing the error.
     */
    message: string;
}

/**
 * Represents the error response returned by the DaaS API with a 404 status code.
 */
interface NotFoundResponse extends ApiErrorResponse {
    /**
     * The Id of the entity that was not found.
     */
    id: number;

    /**
     * The type of entity that was not found.
     */
    entityType: string;
}