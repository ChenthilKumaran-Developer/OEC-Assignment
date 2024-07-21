const api_url = "http://localhost:10010";

export const startPlan = async () => {
    const url = `${api_url}/Plan`;
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({}),
    });

    if (!response.ok) throw new Error("Failed to create plan");

    return await response.json();
};

export const addProcedureToPlan = async (planId, procedureId) => {
    const url = `${api_url}/Plan/AddProcedureToPlan`;
    var command = { planId: planId, procedureId: procedureId };
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(command),
    });

    if (!response.ok) throw new Error("Failed to create plan");

    return true;
};

export const getProcedures = async () => {
    const url = `${api_url}/Procedures`;
    const response = await fetch(url, {
        method: "GET",
    });

    if (!response.ok) throw new Error("Failed to get procedures");

    return await response.json();
};

export const getPlanProcedures = async (planId) => {
    const url = `${api_url}/PlanProcedure?$filter=planId eq ${planId}&$expand=procedure`;
    const response = await fetch(url, {
        method: "GET",
    });

    if (!response.ok) throw new Error("Failed to get plan procedures");

    return await response.json();
};

export const getUsers = async () => {
    const url = `${api_url}/Users`;
    const response = await fetch(url, {
        method: "GET",
    });

    if (!response.ok) throw new Error("Failed to get users");

    return await response.json();
};
//New Method Added

//https://localhost:10011/PlanProcedure/UserAssign/2
export const getUserAssign = async (procedureId) => {
    const url = `${api_url}/PlanProcedure/UserAssign/${procedureId}`;
    const response = await fetch(url, {
        method: "GET",
    });

    if (!response.ok) throw new Error("Failed to get assigned users to procedure");

    return await response.json();
};


//http://localhost:10010/PlanProcedure/UsersAssignToPlanProcedure
export const UserAssign = async (planId, procedureId, users) => {
    const url = `${api_url}/PlanProcedure/UsersAssignToPlanProcedure`;
    const command = {
        planId: planId,
        procedureId: procedureId,
        userIds: users.map(user => ({ userId: user.userId })),
    };
    const response = await fetch(url, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(command),
    });
    if (!response.ok) throw new Error("Failed to assign users to procedure");
    return await response.json();
};

//http://localhost:10010/PlanProcedure/DeleteAssignUser
export const deleteUserAssign = async (userProcedureId) => {
    const url = `${api_url}/PlanProcedure/DeleteAssignUser`;
    const command = { userProcedureId: userProcedureId };
    const response = await fetch(url, {
        method: "DELETE",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(command),
    });
    if (!response.ok) throw new Error("Failed to delete assigned user");
    return await response.json();
};
